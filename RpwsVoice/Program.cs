using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using RpwsVoice.PersistEntities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice
{
    class Program
    {
        public static Random rand = new Random();
        public static RpwsVoiceConfig config;
        public static LiteDatabase db;

        static void Main(string[] args)
        {
            //Open config
            string configPath = @"E:\RPWS_Production\config\voice\voice.json"; //Todo: Get this from the args passed. This is just temporary during testing
            config = JsonConvert.DeserializeObject<RpwsVoiceConfig>(File.ReadAllText(configPath));
            
            //Open database
            db = new LiteDatabase("database.db");
            
            //Start server
            MainAsync().GetAwaiter().GetResult();
        }

        public static LiteCollection<VoiceToken> GetVoiceTokensCollection()
        {
            return db.GetCollection<VoiceToken>("voice_tokens");
        }

        public static LiteCollection<VoiceAccount> GetAccountsCollection()
        {
            return db.GetCollection<VoiceAccount>("accounts");
        }

        public static VoiceToken AuthenticateVoiceToken(string token)
        {
            var collec = GetVoiceTokensCollection();
            var foundTokens = collec.Find(x => x._id == token).ToArray();
            if (foundTokens.Length == 1)
                return foundTokens[0];
            return null;
        }

        static Task MainAsync()
        {
            //Start server
            Console.WriteLine("Starting HTTP server...");
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    IPAddress addr = IPAddress.Any;
                    options.Listen(addr, config.serverPort);
                })
                .UseStartup<Program>()
                .Build();

            return host.RunAsync();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(HttpHandler.OnHttpRequest);
        }

        public static Task QuickWriteToDoc(Microsoft.AspNetCore.Http.HttpContext context, string content, string type = "text/html", int code = 200)
        {
            var response = context.Response;
            response.StatusCode = code;
            response.ContentType = type;

            //Load the template.
            string html = content;
            var data = Encoding.UTF8.GetBytes(html);
            response.ContentLength = data.Length;
            return response.Body.WriteAsync(data, 0, data.Length);
        }

        public static Task QuickWriteJsonToDoc<T>(Microsoft.AspNetCore.Http.HttpContext context, T data, int code = 200)
        {
            string reply = JsonConvert.SerializeObject(data, Formatting.None);
            return QuickWriteToDoc(context, reply, "application/json", code);
        }

        public static string GenerateRandomString(int length)
        {
            string output = "";
            char[] chars = "qwertyuiopasdfghjklzxcvbnm".ToCharArray();
            for (int i = 0; i < length; i++)
            {
                output += chars[rand.Next(0, chars.Length)];
            }
            return output;
        }
    }
}
