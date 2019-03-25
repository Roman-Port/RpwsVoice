using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.PersistEntities
{
    public class VoiceAccount
    {
        public string _id { get; set; } //The RPWS user ID

        public float credits { get; set; } //The number of credits 
        public long last_token_awarded_time { get; set; } //The last time, UTC, that tokens were rewarded.
        
        public int total_failed_requests { get; set; }
        public int total_ok_requests { get; set; }

        public bool b_has_sent_welcome_email { get; set; } //If this is false, send welcome email.
    }
}
