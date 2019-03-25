using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.VoiceService.Entities
{
    public class RequestData_CmdDict
    {
        public string dictation_type { get; set; }
        public string dictation_language { get; set; }
        public string locale { get; set; }
        public string application_name { get; set; }
        public string organization_id { get; set; }
        public string phone_OS { get; set; }
        public string phone_network { get; set; }
        public string audio_source { get; set; }
        public string application_session_id { get; set; }
        public string utterance_number { get; set; }
        public string ui_language { get; set; }
    }

    public class RequestData
    {
        public string appKey { get; set; }
        public string appId { get; set; }
        public string uId { get; set; }
        public string inCodec { get; set; }
        public string outCodec { get; set; }
        public string cmdName { get; set; }
        public string appName { get; set; }
        public string appVersion { get; set; }
        public string language { get; set; }
        public int cmdTimeout { get; set; }
        public string carrier { get; set; }
        public string deviceModel { get; set; }
        public RequestData_CmdDict cmdDict { get; set; }
    }
}
