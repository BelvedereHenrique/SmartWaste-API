using Moq;
using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Library.Tests
{
    public static class SecurityManagerHelper
    {
        private static Guid UserID = Guid.NewGuid();
        private static Guid PersonID = Guid.NewGuid();
        private static string Login = Guid.NewGuid().ToString();

        public static Mock<ISecurityManager<IdentityContract>> GetUnauthenticatedIdentity()
        {
            var identity = new IdentityContract()
            {
                IsAuthenticated = false,
                Person = null,
                User = null,
                AuthenticationType = string.Empty,
                Login = string.Empty
            };
            
            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            securityManager.Setup(x => x.User).Returns(identity);
            
            return securityManager;
        }

        public static Mock<ISecurityManager<IdentityContract>> GetAuthenticatedIdentity(PersonContract person, UserContract user, List<string> roles)
        {
            var identity = new IdentityContract()
            {
                IsAuthenticated = true,
                Person = person,
                User = user,
                AuthenticationType = "token",
                Login = Login
            };

            roles.ForEach(r => identity.User.Roles.Add(new SmarteWaste_API.Contracts.Role.RoleContract()
            {
                Name = r
            }));

            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            securityManager.Setup(x => x.User).Returns(identity);

            securityManager.Setup(x => x.IsInRole(It.Is((string r) => roles.Any(role => role == r)))).Returns(true);
            securityManager.Setup(x => x.IsInRole(It.Is((string r) => roles.All(role => role != r)))).Returns(false);

            return securityManager;
        }

        public static UserContract GetUserContract()
        {
            return new UserContract() {
                ExpirationDate = null,
                ID = UserID,
                Login = Login,
                Password = Guid.NewGuid().ToString(),
                Roles = new List<SmarteWaste_API.Contracts.Role.RoleContract>()
            };
        }

        public static PersonContract GetPersonContract(bool companyUser)
        {
            var person = new PersonContract()
            {
                Email = Guid.NewGuid().ToString(),
                ID = PersonID,
                Name = Guid.NewGuid().ToString(),
                PersonType = PersonTypeEnum.LegalPerson,
                UserID = UserID
            };

            if (companyUser)
                person.CompanyID = Guid.NewGuid();

            return person;
        }
    }
}
