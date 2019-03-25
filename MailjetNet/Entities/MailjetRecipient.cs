using System;
using System.Collections.Generic;
using System.Text;

namespace MailjetNet.Entities
{
    public class MailjetRecipient
    {
        public MailjetRecipient(string name, string email)
        {
            Email = email;
            Name = name;
        }

        public string Name;
        public string Email;
    }
}
