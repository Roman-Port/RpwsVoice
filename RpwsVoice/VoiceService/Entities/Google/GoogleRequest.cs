using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.VoiceService.Entities.Google
{
    public class GoogleRequest
    {
        public GoogleRequest_Config config;
        public GoogleRequest_Audio audio;
    }

    public class GoogleRequest_Config
    {
        public string encoding;
        public int sampleRateHertz;
        public string languageCode;
        public bool enableWordTimeOffsets;
    }

    public class GoogleRequest_Audio
    {
        public string content;
    }
}
