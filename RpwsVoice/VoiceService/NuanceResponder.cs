using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice.VoiceService
{
    public static class NuanceResponder
    {
        public static Task RespondWithData(Microsoft.AspNetCore.Http.HttpContext e, string content)
        {
            //This is really janky, but it seems to be happy enough. Set the content type and headers and then provide the fake form data.
            e.Response.Headers.Add("Nuance-SessionId", "c1696372-f396-492d-8ff7-9321e37018df");
            e.Response.Headers.Add("Nuance-Context", "8a89f327-5643-4b21-8671-3ee617f8a242");

            //Create message
            string message = "----Nuance_NMSP_vutc5w1XobDdefsYG3wq\r\nContent-Disposition: form-data; name=\"QueryResult\"\r\nContent-Type: application/JSON; charset=utf-8\r\nNuance-Context: 8a89f327-5643-4b21-8671-3ee617f8a242\r\n\r\n" + content + "\r\n----Nuance_NMSP_vutc5w1XobDdefsYG3wq--\r\n";
            return Program.QuickWriteToDoc(e, message, "application/multipart/form-data; boundary=--", 200);
        }
    }
}
