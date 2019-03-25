using Newtonsoft.Json;
using RpwsVoice.PersistEntities;
using RpwsVoice.RpwsAuth.Entities;
using RpwsVoice.VoiceService.Entities;
using RpwsVoice.VoiceService.Entities.FakeNuance;
using RpwsVoice.VoiceService.Entities.Google;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice.VoiceService
{
    public static class VoiceServiceRequestHandler
    {
        public static async Task OnHttpRequest(Microsoft.AspNetCore.Http.HttpContext e)
        {
            //Authenticate this user using the hostname used. We must use the hostname because that's the most I can change from the Pebble configuration.
            UrlParams urlParams = RpwsAuth.DecodeUrlParams.DecodeUrl(e.Request.Host.Host);

            //Check the voice token against our database
            VoiceToken me = Program.AuthenticateVoiceToken(urlParams.accessToken);
            if (me == null)
                throw new Exception("Invalid access token!");

            //Grab their user account
            VoiceAccount account = RpwsQuota.RpwsVoiceAuth.GetAccount(me.rpws_uid);

            //Reward daily credits
            DateTime lastTimeCreditsAwarded = new DateTime(account.last_token_awarded_time);
            DateTime nowTime = DateTime.UtcNow;
            double daysSinceLastUpdate = (nowTime - lastTimeCreditsAwarded).TotalDays;
            account.credits += (float)daysSinceLastUpdate * Program.config.dailyCreditsGiven;
            account.last_token_awarded_time = nowTime.Ticks;

            //Cap the daily credits
            if (account.credits > Program.config.maxDailyRefillCredits)
                account.credits = Program.config.maxDailyRefillCredits;

            //Send the welcome email if we haven't
            /*if(!account.b_has_sent_welcome_email)
                RpwsAuth.EmailService.SendWelcomeEmail(me);
            account.b_has_sent_welcome_email = true;*/

            //Save account and log
            Console.WriteLine($"Voice request by {account._id}, credits remaining: {account.credits}");
            RpwsQuota.RpwsVoiceAuth.SaveAccount(account);

            //Stop request if needed
            if (account.credits < 1)
                throw new Exception("Quota reached.");

            //Pass this into the HTTPDecoder
            Tuple<RequestData, List<Stream>> payload = VoiceService.HttpDecoder.DecodeHttpData(e).Result;
            RequestData requestConfig = payload.Item1;

            //Now, convert this to base64 request
            string audioData = VoiceService.SpeexWithHeaderByteConverter.CreateBase64Payload(payload.Item2);

            //Now, form a Google request and send it.
            GoogleReply textData = await VoiceService.GoogleRequester.DoRequest(audioData, urlParams.languageRegion, Program.config.googleApiKey);

            //Now, convert to a fake Nuance response
            List<GoogleReply_Result> results = new List<GoogleReply_Result>();
            bool ok = true;
            if (textData != null)
            {
                results = textData.results;
                ok = false;
            } else
            {
                ok = textData.results.Count != 0;
            }

            //Save
            account.credits -= 0.5f;
            if(ok)
                account.credits -= 0.5f;
            RpwsQuota.RpwsVoiceAuth.SaveAccount(account);

            //Now, form a reply that the Pebble will be happy with.
            NuanceResponse reply = VoiceService.FakeNuanceResponse.ConvertResponse(results, requestConfig);

            //Write this out
            await VoiceService.NuanceResponder.RespondWithData(e, JsonConvert.SerializeObject(reply));
        }
    }
}
