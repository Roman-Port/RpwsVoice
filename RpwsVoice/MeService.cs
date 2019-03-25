using RpwsVoice.PersistEntities;
using RpwsVoice.RpwsAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice
{
    public static class MeService
    {
        public static async Task OnHttpRequest(Microsoft.AspNetCore.Http.HttpContext e)
        {
            //Try and authenticate the user. 
            VoiceAccount va = null;
            if(e.Request.Query.ContainsKey("key"))
            {
                //This is the voice token key
                VoiceToken vc = Program.AuthenticateVoiceToken(e.Request.Query["key"]);
                if (vc == null)
                    throw new Exception("RPWS voice token not valid.");
                va = RpwsQuota.RpwsVoiceAuth.GetAccount(vc.rpws_uid);
            }
            if(e.Request.Query.ContainsKey("token"))
            {
                //This is the RPWS token
                RpwsMe me = await RpwsAuth.RpwsRequests.AuthenticateUser(e.Request.Query["token"]);
                if (me == null)
                    throw new Exception("RPWS token is not valid.");
                va = RpwsQuota.RpwsVoiceAuth.GetAccount(me.uuid);

                //That might've failed. If it did, create a "fake" account
                if(va == null)
                {
                    va = new VoiceAccount
                    {
                        b_has_sent_welcome_email = false,
                        credits = Program.config.defaultCredits,
                        last_token_awarded_time = DateTime.UtcNow.Ticks,
                        total_failed_requests = 0,
                        total_ok_requests = 0,
                        _id = me.uuid
                    };
                }
            }

            //If it failed, stop
            if (va == null)
                throw new Exception("Not authenticated!");

            //Award new tokens for an updated result
            RpwsQuota.RpwsVoiceAuth.AwardNewCredits(va);

            //Write
            await Program.QuickWriteJsonToDoc(e, va);
        }
    }
}
