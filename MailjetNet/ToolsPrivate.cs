using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MailjetNet
{
    class ToolsPrivate
    {
        public static T SendPost<T>(object body, string apiToken)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.mailjet.com/v3.1/send");
            string ser = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var data = Encoding.ASCII.GetBytes(ser);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + apiToken);
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            return JsonConvert.DeserializeObject<T>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    }
}
