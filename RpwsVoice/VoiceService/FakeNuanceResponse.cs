using RpwsVoice.VoiceService.Entities;
using RpwsVoice.VoiceService.Entities.FakeNuance;
using RpwsVoice.VoiceService.Entities.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.VoiceService
{
    public static class FakeNuanceResponse
    {
        public static NuanceResponse ConvertResponse(List<GoogleReply_Result> googleData, RequestData request)
        {
            NuanceResponse r = new NuanceResponse();
            //First, write the "audio_transfer_info" data.
            r.audio_transfer_info = new NuanceResponseTransferInfo();
            //This is misisng the packages meta. We're gonna hope the Pebble app doesn't care about this.
            //Set the context to the one we were told by the app.
            r.NMAS_PRFX_SESSION_ID = request.cmdDict.application_session_id;
            //Add the one transcription to the transcriptions.
            GoogleReply_Result trans = new GoogleReply_Result();
            if (googleData.Count > 0)
            {
                trans = googleData[0];
            }
            else
            {
                trans = new GoogleReply_Result();
                trans.alternatives = new List<GoogleReply_Alternative>();
            }
            r.transcriptions = new string[trans.alternatives.Count];
            r.confidences = new int[trans.alternatives.Count];
            for (int i = 0; i < trans.alternatives.Count; i++)
            {
                r.transcriptions[i] = trans.alternatives[i].transcript;
                r.confidences[i] = 100;//trans.alternatives[i].confidence;
            }
            //Now, add each word in each alternatives.
            r.words = new NuanceResponseWord[trans.alternatives.Count][];
            for (int alt = 0; alt < trans.alternatives.Count; alt++)
            {
                var thisTrans = trans.alternatives[alt];
                string[] words = thisTrans.transcript.Split(' ');
                float conf = thisTrans.confidence;
                r.words[alt] = new NuanceResponseWord[words.Length];
                for (int word = 0; word < words.Length; word++)
                {
                    var w = new NuanceResponseWord();

                    w.word = words[word];
                    w.confidence = conf.ToString();

                    r.words[alt][word] = w;
                }
            }

            return r;
        }
    }
}
