using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SmartWaste_API.Security
{
    public class UnauthorizedUserPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get
            {
                return null;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public class UnauthorizedUserIdentity : IIdentity
        {
            public string AuthenticationType
            {
                get
                {
                    return null;
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return null;
                }
            }
        }
    }
}