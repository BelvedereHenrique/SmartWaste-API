﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Password
{
    public class PasswordContract
    {
        public string Email { get; set; }
        public string  Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Token { get; set; }
    }
}
