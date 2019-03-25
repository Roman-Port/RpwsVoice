using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.PersistEntities
{
    //Voice tokens link to RPWS tokens and also store the user data. 
    public class VoiceToken
    {
        public string rpws_token { get; set; } //RPWS token registered

        public string rpws_uid { get; set; } //RPWS user id
        public string rpws_email { get; set; } //RPWS email
        public string rpws_name { get; set; } //RPWS name
        public long last_cache_refresh_time { get; set; } //Last time the above section was updated

        public string _id { get; set; } //The token for this
    }
}
