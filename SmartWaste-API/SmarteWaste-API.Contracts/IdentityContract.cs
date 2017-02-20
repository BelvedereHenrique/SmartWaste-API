﻿using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts
{
    public class IdentityContract : IIdentityModel
    {
        public IdentityContract()
        {
            Roles = new List<string>();
        }

        public bool IsAuthenticated { get; set; }
        public string Login { get; set; }
        public string AuthenticationType { get; set; }
        public UserContract User { get; set; }
        public PersonContract Person { get; set; }
        public List<string> Roles { get; set; }
    }
}
