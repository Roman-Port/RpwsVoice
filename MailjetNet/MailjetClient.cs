using MailjetNet.Entities;
using MailjetNet.JsonEntities;
using System;
using System.Net;

namespace MailjetNet
{
    public class MailjetClient
    {
        public MailjetClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        private readonly string apiKey;

        public bool SendEmail(Entities.MailjetEmail email)
        {
            try
            {
                //Convert this into the group the API can understand.
                EmailGroup group = new EmailGroup();
                group.Messages = new Entities.MailjetEmail[] { email };
                //Serialize this to JSON and send it to the API.
                EmailGroup reply = ToolsPrivate.SendPost<EmailGroup>(group, apiKey);
                //We can tell if it completed if all emails have good status.
                foreach (MailjetEmail e in reply.Messages)
                {
                    if (e.Status != "success")
                        return false;
                }
                return true;
            } catch (WebException wex)
            {
                Console.WriteLine(wex.Message);
                return false;
            }
        }
    }
}
