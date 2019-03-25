using RpwsVoice.RpwsAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.RpwsAuth
{
    public class DecodeUrlParams
    {
        public static UrlParams DecodeUrl(string hostname)
        {
            UrlParams output = new UrlParams();
            //Check to make sure the hostname is even correct.
            //Hostname: en_US-token-prod.voice.get-rpws.com
            if (!hostname.EndsWith(".voice.get-rpws.com"))
                throw new Exception("Incorrect domain. Hostname must end with \".voice.get-rpws.com\".");

            //Trim off of the end ".voice.get-rpws.com".
            hostname = hostname.Substring(0, hostname.Length - ".voice.get-rpws.com".Length);
            //Hostname: en_US-token-prod

            //Split the URL into parts
            string[] parts = hostname.Split('-');

            //Parse the language region.
            output.languageRegion = ParseLangRegion(parts[0]);

            //Grab others
            output.accessToken = parts[1]; //Uh oh
            output.enviornment = parts[2];

            return output;
        }

        static string ParseLangRegion(string input)
        {
            if(input.Length != 4)
                throw new Exception("Malformed language-region code. The region code must be in the form: \"xxXX\".");

            //Convert to new form
            return input.Substring(0, 2).ToLower() + "_" + input.Substring(2, 2).ToUpper();
        }
    }
}
