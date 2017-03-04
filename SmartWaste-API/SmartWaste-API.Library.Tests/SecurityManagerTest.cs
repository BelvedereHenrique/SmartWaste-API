using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using System.Security.Principal;
using Moq;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace SmartWaste_API.Library.Tests
{
    [TestClass]
    public class SecurityManagerTest
    {
        [TestMethod]
        public void SecurityUserWhenUserIsNotLogged()
        {
            var identity = new Mock<IIdentity>();
            identity.Setup(x => x.IsAuthenticated).Returns(false);

            var principal = new Mock<IPrincipal>();
            principal.Setup(x => x.Identity).Returns(identity.Object);

            var securityManager = (Security.ISecurityManager<IdentityContract>)new SecurityManager<IdentityContract>(principal.Object);

            Assert.IsNotNull(securityManager);
            Assert.IsNotNull(securityManager.User);
            Assert.IsFalse(securityManager.User.IsAuthenticated);
            Assert.IsNotNull(securityManager.User.Roles);
            Assert.IsFalse(securityManager.IsInRole(Guid.NewGuid().ToString()));
        }

        [TestMethod]
        public void SecurityUserWhenUserIsLogged()
        {
            var authenticateType = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();

            var identityContract = GetIdentityContract();

            var claims = new List<Claim>()
            {
                new Claim("data", JsonConvert.SerializeObject(identityContract))
            };

            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(x => x.IsAuthenticated).Returns(true);
            identity.Setup(x => x.AuthenticationType).Returns(authenticateType);
            identity.Setup(x => x.Name).Returns(name);
            identity.Setup(x => x.Claims).Returns(claims);

            var principal = new Mock<IPrincipal>();
            principal.Setup(x => x.Identity).Returns(identity.Object);

            var securityManager = (Security.ISecurityManager<IdentityContract>)new SecurityManager<IdentityContract>(principal.Object);

            Assert.IsNotNull(securityManager);
            Assert.IsNotNull(securityManager.User);
            Assert.IsTrue(securityManager.User.IsAuthenticated);

            Assert.AreEqual(identityContract.Person.CompanyID, securityManager.User.Person.CompanyID);
            Assert.AreEqual(identityContract.Person.Email, securityManager.User.Person.Email);
            Assert.AreEqual(identityContract.Person.ID, securityManager.User.Person.ID);
            Assert.AreEqual(identityContract.Person.Name, securityManager.User.Person.Name);
            Assert.AreEqual(identityContract.Person.PersonType, securityManager.User.Person.PersonType);
            Assert.AreEqual(identityContract.Person.UserID, securityManager.User.Person.UserID);

            Assert.AreEqual(identityContract.User.ExpirationDate, securityManager.User.User.ExpirationDate);
            Assert.AreEqual(identityContract.User.ID, securityManager.User.User.ID);
            Assert.AreEqual(identityContract.User.Login, securityManager.User.User.Login);
            Assert.AreEqual(identityContract.User.Password, securityManager.User.User.Password);
            Assert.AreEqual(identityContract.User.RecoveredOn, securityManager.User.User.RecoveredOn);
            Assert.AreEqual(identityContract.User.RecoveryToken, securityManager.User.User.RecoveryToken);
            Assert.AreEqual(identityContract.User.Roles.Count(), securityManager.User.User.Roles.Count());

            Assert.AreEqual(identityContract.AuthenticationType, securityManager.User.AuthenticationType);
            Assert.AreEqual(identityContract.Login, securityManager.User.Login);

            Assert.IsFalse(securityManager.IsInRole(Guid.NewGuid().ToString()));
            Assert.IsTrue(securityManager.IsInRole(identityContract.Roles.First()));
            Assert.IsTrue(securityManager.IsInRole(identityContract.Roles.Last()));

        }

        private IdentityContract GetIdentityContract()
        {
            return new IdentityContract()
            {
                Login = Guid.NewGuid().ToString(),
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    CompanyID = Guid.NewGuid(),
                    Email = Guid.NewGuid().ToString(),
                    ID = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    PersonType = SmarteWaste_API.Contracts.Person.PersonTypeEnum.PrivatePerson,
                    UserID = Guid.NewGuid()
                },
                User = new SmarteWaste_API.Contracts.User.UserContract()
                {
                    ExpirationDate = DateTime.Now,
                    ID = Guid.NewGuid(),
                    Login = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                    RecoveredOn = DateTime.Now,
                    RecoveryToken = Guid.NewGuid().ToString(),
                    Roles = new List<SmarteWaste_API.Contracts.Role.RoleContract>() {
                        new SmarteWaste_API.Contracts.Role.RoleContract() {
                            ID = Guid.NewGuid(),
                            Name = Guid.NewGuid().ToString()
                        },
                        new SmarteWaste_API.Contracts.Role.RoleContract() {
                            ID = Guid.NewGuid(),
                            Name = Guid.NewGuid().ToString()
                        }
                    }
                }
            };
        }
    }
}
