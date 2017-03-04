using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Library;
using SmarteWaste_API.Contracts.Person;

namespace SmartWaste_API.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserService _userService;
        private readonly IPersonService _personService;

        private const string USER_DOESNT_EXIST = "There is no user with this login.";
        private const string USER_MUST_RECOVERY_PASSWORD = "You must recovery your password before login.";
        private const string PASSWORD_DOESNT_MATCH = "The password doesn't match.";

        public SecurityService(IUserService userService, IPersonService personService)
        {
            _userService = userService;
            _personService = personService;
        }

        public OperationResult<IdentityContract> SignIn(string login, string password)
        {
            var result = new OperationResult<IdentityContract>();

            var user = _userService.Get(new UserFilterContract()
            {
                Login = login
            });

            if (user == null)
                result.AddError(USER_DOESNT_EXIST);

            if (!result.Success) return result;

            if (!String.IsNullOrWhiteSpace(user.RecoveryToken) && !user.RecoveredOn.HasValue)
                result.AddError(USER_MUST_RECOVERY_PASSWORD);

            if (!result.Success) return result;

            if (!MD5Helper.Check(user.Password, password))
                result.AddError(PASSWORD_DOESNT_MATCH);

            if (!result.Success) return result;

            var person = _personService.Get(new PersonFilterContract()
            {
                UserID = user.ID
            });

            result.Result = new IdentityContract()
            {
                User = user,
                Person = person
            };

            return result;
        }
    }
}
