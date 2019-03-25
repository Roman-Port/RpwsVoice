using Newtonsoft.Json;
using RpwsVoice.RpwsAuth.Entities;
using RpwsVoice.VoiceService.Entities;
using RpwsVoice.VoiceService.Entities.FakeNuance;
using RpwsVoice.VoiceService.Entities.Google;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice
{
    public static class HttpHandler
    {
        public static async Task OnHttpRequest(Microsoft.AspNetCore.Http.HttpContext e)
        {
            try
            {
                //Choose where to go
                if (e.Request.Path == "/obtain_voice_token")
                {
                    await VoiceTokenService.OnHttpRequest(e);
                    return;
                }
                if (e.Request.Path == "/me")
                {
                    await MeService.OnHttpRequest(e);
                    return;
                }

                //If we aren't sure, go over
                await VoiceService.VoiceServiceRequestHandler.OnHttpRequest(e);
            } catch (Exception ex)
            {
                await Program.QuickWriteToDoc(e, ex.Message, "text/plain", 500);
            }

        }
    }
}
