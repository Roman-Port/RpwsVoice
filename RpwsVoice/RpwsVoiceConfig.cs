using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice
{
    public class RpwsVoiceConfig
    {
        public string googleApiKey;
        public string mailjetApiKey;

        public int serverPort;

        public float dailyCreditsGiven;
        public float defaultCredits;
        public float maxDailyRefillCredits;
    }
}
