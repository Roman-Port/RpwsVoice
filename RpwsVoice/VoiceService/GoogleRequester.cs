using Newtonsoft.Json;
using RpwsVoice.VoiceService.Entities.Google;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice.VoiceService
{
    public static class GoogleRequester
    {
        public static async Task<GoogleReply> DoRequest(string audioPayload, string lang, string apiKey)
        {
            //Create payload
            string payload = CreateRequestPayload(audioPayload, lang);

            //Submit to Google
            string reply;
            using(HttpClient hc = new HttpClient())
            {
                var content = new StringContent(payload);
                var response = await hc.PostAsync("https://speech.googleapis.com/v1/speech:recognize?key="+apiKey, content);
                reply = await response.Content.ReadAsStringAsync();
            }

            //Decode this and return the results.
            GoogleReply replyData = JsonConvert.DeserializeObject<GoogleReply>(reply);
            return replyData;
        }

        static string CreateRequestPayload(string audioPayload, string lang = "en-US")
        {
            //Create the Google request JSON
            GoogleRequest request = new GoogleRequest
            {
                audio = new GoogleRequest_Audio
                {
                    content = audioPayload
                },
                config = new GoogleRequest_Config
                {
                    enableWordTimeOffsets = false,
                    encoding = "SPEEX_WITH_HEADER_BYTE",
                    languageCode = lang,
                    sampleRateHertz = 16000
                }
            };

            //Convert to JSON
            return JsonConvert.SerializeObject(request);
        }
    }
}
