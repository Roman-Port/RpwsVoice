﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RpwsVoice.RpwsAuth.Entities
{
    public class RpwsMe
    {
        //User info
        public string email { get; set; }
        public string googleId { get; set; }
        public long registrationDate { get; set; }
        public string uuid { get; set; } //Used to identify ourselves internally.
        public string legacyPebbleId { get; set; } //OLD Pebble ID from the original Pebble servers
        public bool isPebbleLinked { get; set; }
        public string pebbleId { get; set; }
        public bool isAppDev { get; set; }
        public string appDevName { get; set; }
        public string name { get; set; }
    }

    public class RpwsMeRequest
    {
        public RpwsMe user;
    }
}
