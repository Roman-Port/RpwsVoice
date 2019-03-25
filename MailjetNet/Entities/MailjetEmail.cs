using System;
using System.Collections.Generic;
using System.Text;

namespace MailjetNet.Entities
{
    public class MailjetEmail
    {
        //Constructors for emails.
        public MailjetEmail(MailjetRecipient from, MailjetRecipient[] recipients, string subject, string text, string html = null, MailjetRecipient[] cc = null, MailjetRecipient[] bcc = null)
        {
            To = recipients;
            From = from;
            Subject = subject;
            TextPart = text;
            HTMLPart = html;
            if (cc == null)
                cc = new MailjetRecipient[0];
            if (bcc == null)
                bcc = new MailjetRecipient[0];
            Cc = cc;
            Bcc = bcc;
        }

        public MailjetRecipient From;
        public MailjetRecipient[] To;
        public MailjetRecipient[] Cc;
        public MailjetRecipient[] Bcc;
        public string Subject;
        public string TextPart;
        public string HTMLPart;

        //The following is only used in the API reply.
        public string Status;

        public bool SendEmail(MailjetClient client)
        {
            //Send it fromk the client.
            return client.SendEmail(this);
        }
    }
}
