using RpwsVoice.PersistEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.RpwsQuota
{
    public static class RpwsVoiceAuth
    {
        public static VoiceAccount GetAccount(string userId)
        {
            var collec = Program.GetAccountsCollection();
            var acc = collec.FindOne(x => x._id == userId);
            if(acc == null)
            {
                acc = new VoiceAccount
                {
                    _id = userId,
                    credits = Program.config.defaultCredits,
                    total_failed_requests = 0,
                    total_ok_requests = 0,
                    last_token_awarded_time = DateTime.UtcNow.Ticks,
                    b_has_sent_welcome_email = false
                };
                collec.Insert(acc);
            }
            return acc;
        }

        public static void SaveAccount(VoiceAccount acc)
        {
            Program.GetAccountsCollection().Update(acc);
        }
    }
}
