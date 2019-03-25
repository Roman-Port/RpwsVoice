using Newtonsoft.Json;
using RpwsVoice.RpwsAuth.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice.RpwsAuth
{
    public class RpwsRequests
    {
        private static async Task<T> SendAuthenticatedRequest<T>(string path, string token)
        {
            string reply;
            using (HttpClient hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await hc.GetAsync("https://blue.api.get-rpws.com"+path);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception("Remove serer returned a non OK result.");
                reply = await response.Content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<T>(reply);
        } 

        public static async Task<RpwsMe> AuthenticateUser(string token)
        {
            try
            {
                return (await SendAuthenticatedRequest<RpwsMeRequest>("/v1/rpws_me", token)).user;
            } catch
            {
                return null;
            }
        }
    }
}
