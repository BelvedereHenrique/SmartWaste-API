using SmarteWaste_API.Contracts;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class SecurityController : ApiController
    {
        private readonly ISecurityManager<IdentityContract> _user;

        public SecurityController(ISecurityManager<IdentityContract> user)
        {
            _user = user;
        }

        [Authorize]
        public IHttpActionResult GetUserInfo()
        {
            try
            {
                return Ok(new JsonModel<SecurityModel>(new SecurityModel(_user.User)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to get user information.");
                return Ok(error);
            }
        }
    }
}