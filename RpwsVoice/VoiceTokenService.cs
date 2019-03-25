using LiteDB;
using RpwsVoice.PersistEntities;
using RpwsVoice.RpwsAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice
{
    public static class VoiceTokenService
    {
        public static async Task OnHttpRequest(Microsoft.AspNetCore.Http.HttpContext e)
        {
            //Grab the token from the URL
            if (!e.Request.Query.ContainsKey("token"))
                throw new Exception("Missing token!");
            string rpws_token = e.Request.Query["token"];

            //Look the token up in the database and see if we already have it.
            var collec = Program.GetVoiceTokensCollection();
            var tokens = collec.Find(x => x.rpws_token == rpws_token).ToArray();

            //If we already have a token for this, return the token
            if (tokens.Length == 1)
            {
                await Program.QuickWriteToDoc(e, tokens[0]._id, "text/plain");
                return;
            }

            //Generate a new token. Request RPWS data
            RpwsMe me = await RpwsAuth.RpwsRequests.AuthenticateUser(rpws_token);
            if (me == null)
                throw new Exception("Failed to authenticate with RPWS.");

            //Create voice token
            string voiceToken = GenerateVoiceToken(collec);
            VoiceToken vt = new VoiceToken
            {
                last_cache_refresh_time = DateTime.UtcNow.Ticks,
                rpws_email = me.email,
                rpws_name = me.name,
                rpws_token = rpws_token,
                rpws_uid = me.uuid,
                _id = voiceToken
            };

            //Insert
            collec.Insert(vt);

            //Log
            Console.WriteLine($"Generated voice token for RPWS user {me.uuid} ({me.email}), {voiceToken}");

            //Write the token
            await Program.QuickWriteToDoc(e, voiceToken, "text/plain");
        }

        const int VOICE_TOKEN_LENGTH = 16;

        private static string GenerateVoiceToken(LiteCollection<VoiceToken> collec)
        {
            string token = Program.GenerateRandomString(VOICE_TOKEN_LENGTH);
            //Check to see if this exists. Also check and see if it contains any words that could trigger web filters
            while (collec.Find(x => x._id == token).Count() != 0 || !RpwsAuth.ProfanityFilter.CheckString(token))
                token = Program.GenerateRandomString(VOICE_TOKEN_LENGTH);
            return token;
        }
    }
}
