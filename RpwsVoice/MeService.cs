using RpwsVoice.PersistEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice
{
    public static class MeService
    {
        public static async Task OnHttpRequest(Microsoft.AspNetCore.Http.HttpContext e)
        {
            //Try and authenticate the user. 
            VoiceAccount va = null;
            //Todo
        }
    }
}
