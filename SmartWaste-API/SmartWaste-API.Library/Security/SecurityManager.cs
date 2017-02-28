using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartWaste_API.Library.Security
{
    public class SecurityManager<T> : ISecurityManager<T> where T : IIdentityModel, new()
    {
        private T _model;
        private IPrincipal _principal;
        private ClaimsIdentity _claimsIdentity;

        public SecurityManager(IPrincipal principal)
        {
            _principal = principal;            
        }

        public T User
        {
            get
            {
                if (_principal == null || !_principal.Identity.IsAuthenticated)
                    return new T() { IsAuthenticated = false };

                if (_claimsIdentity == null)
                    _claimsIdentity = _principal.Identity as ClaimsIdentity;

                if(_model == null)
                    _model = ClaimsParser.Parse<T>(_claimsIdentity);

                return _model;
            }
        }

        public bool IsInRole(string roleName)
        {
            return User.Roles.Any(x => x.ToLower().Trim() == roleName.ToLower().Trim());
        }
    }
}
