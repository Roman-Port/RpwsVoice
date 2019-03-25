using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.VoiceService.Entities.FakeNuance
{
    
    public class NuanceResponse
    {
        
        public int final_response = 1;
        
        public string result_format = "rec_text_results";
        
        public string prompt = "";
        
        public int status_code = 0;
        
        public int[] confidences; //Same index as transcriptions
        
        public string[] transcriptions;
        
        public string cadence_regulatable_result = "completeRecognition";
        
        public NuanceResponseWord[][] words;
        
        public string NMAS_PRFX_TRANSACTION_ID = "2";
        
        public string NMAS_PRFX_SESSION_ID; //The ID requested
        
        public NuanceResponseTransferInfo audio_transfer_info;
        
        public string result_type = "NMDP_ASR_CMD";
    }

    
    public class NuanceResponseWord
    {
        
        public string confidence; //Between 0 and 1.
        
        public string word; //End with "\\*no-space-before" if this is the beginning of a sentance.
    }

    
    public class NuanceResponseTransferInfo
    {
        
        public int audio_id = 658;
        
        public string nss_server = "172.16.59.95:4516";
        
        public string end_time = "20180817021656168";
        
        public string start_time = "20180817021653827";
        
        public NuanceResponseTransferInfoPackage[] packages;
    }

    
    public class NuanceResponseTransferInfoPackage
    {
        
        public int bytes;
        
        public string time;
    }
}
