using MailjetNet;
using MailjetNet.Entities;
using RpwsVoice.PersistEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.RpwsAuth
{
    public class EmailService
    {
        public static bool SendEmail(string userName, string emailAddress, string subject, string text)
        {
            MailjetClient client = new MailjetClient(Program.config.mailjetApiKey);
            MailjetRecipient sender = new MailjetRecipient("RPWS Voice", "noreply@get-rpws.com");
            MailjetRecipient recipient = new MailjetRecipient(userName, emailAddress);
            MailjetEmail email = new MailjetEmail(sender, new MailjetRecipient[] { recipient }, subject, text, text);
            return client.SendEmail(email);
        }

        public static void SendWelcomeEmail(VoiceToken vt)
        {
            SendEmail(vt.rpws_name, vt.rpws_email, "Welcome to RPWS Voice!", $"Hi there!\n\nYou just used the new RPWS voice feature for the first time. RPWS uses Google speech to text, a paid voice dictation service, to do this. To keep RPWS free, you'll have a limited number of dictations available.\n\nYou were given {Program.config.defaultCredits} credits to spend on voice dictation. You'll be given {Program.config.dailyCreditsGiven} credits each day. If you run out of credits, you won't be able to use dictation until the next day. This daily quota is not final and may be changed in the future. Currently, there are no plans to add a paid option.\n\nThanks for using RPWS!");
        }
    }
}
