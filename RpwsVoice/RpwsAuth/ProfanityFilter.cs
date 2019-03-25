using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.RpwsAuth
{
    public static class ProfanityFilter
    {
        static string[] profanities = new string[]
        {
            "fuck",
            "shit",
            "porn",
            "hentai",
            "damn",
            "nigger",
            "nig",
            "ass",
            "bomb",
            "gun"
        };

        public static bool CheckString(string input)
        {
            input = input.ToLower();
            foreach(string p in profanities)
            {
                if (input.Contains(p))
                    return false;
            }
            return true;
        }
    }
}
