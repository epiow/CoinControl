﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinControl
{
    public static class AuthenticationManager
    {
        public static int LoggedInUserId { get; set; }
    }
    public class User
    {
        public long User_ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
