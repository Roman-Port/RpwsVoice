using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.VoiceService.Entities.Google
{
    public class GoogleReply_Alternative
    {
        public string transcript { get; set; }
        public float confidence { get; set; }
    }

    public class GoogleReply_Result
    {
        public List<GoogleReply_Alternative> alternatives { get; set; }
    }

    public class GoogleReply
    {
        public List<GoogleReply_Result> results { get; set; }
    }
}
