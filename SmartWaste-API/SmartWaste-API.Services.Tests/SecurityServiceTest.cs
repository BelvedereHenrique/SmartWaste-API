using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Services.Interfaces;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Services.Security;
using System.Linq;
using SmartWaste_API.Library;
using SmarteWaste_API.Contracts.Person;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class SecurityServiceTest
    {
        [TestMethod]
        public void SignInWhenUserDoesntExist()
        {
            var login = Guid.NewGuid().ToString();
            var passowrd = Guid.NewGuid().ToString();

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>()));

            var service = (ISecurityService)new SecurityService(userService.Object, null);
            var result = service.SignIn(login, passowrd);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.AreEqual(result.Messages.Count, 1);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.Login == login)), Times.Once);
        }

        [TestMethod]
        public void SignInWhenUserMustRecoveryPassword()
        {
            var login = Guid.NewGuid().ToString();
            var passowrd = Guid.NewGuid().ToString();

            var user = new UserContract()
            {
                Login = login,
                Password = passowrd,
                RecoveryToken = Guid.NewGuid().ToString()
            };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(user);

            var service = (ISecurityService)new SecurityService(userService.Object, null);
            var result = service.SignIn(login, passowrd);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.AreEqual(result.Messages.Count, 1);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.Login == login)), Times.Once);
        }

        [TestMethod]
        public void SignInWhenPasswordIsWrong()
        {
            var login = Guid.NewGuid().ToString();
            var passowrd = Guid.NewGuid().ToString();

            var user = new UserContract()
            {
                Login = login,
                Password = passowrd
            };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(user);

            var service = (ISecurityService)new SecurityService(userService.Object, null);
            var result = service.SignIn(login, passowrd);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.AreEqual(result.Messages.Count, 1);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.Login == login)), Times.Once);
        }

        [TestMethod]
        public void SignInSuccessTest()
        {
            var login = Guid.NewGuid().ToString();
            var passowrd = Guid.NewGuid().ToString();

            var user = new UserContract()
            {
                ID = Guid.NewGuid(),
                Login = login,
                Password = MD5Helper.Create(passowrd),
                Roles = new System.Collections.Generic.List<SmarteWaste_API.Contracts.Role.RoleContract>() {
                    new SmarteWaste_API.Contracts.Role.RoleContract() {
                        ID = Guid.NewGuid(),
                        Name = Guid.NewGuid().ToString()
                    }
                }
            };

            var person = new PersonContract()
            {
                ID = Guid.NewGuid(),
                CompanyID = null
            };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(user);

            var personService = new Mock<IPersonService>();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(person);

            var service = (ISecurityService)new SecurityService(userService.Object, personService.Object);
            var result = service.SignIn(login, passowrd);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.AreEqual(result.Result.User, user);
            Assert.AreEqual(result.Result.Person, person);
            Assert.AreEqual(result.Result.Roles.Count(), user.Roles.Count);
            Assert.AreEqual(result.Result.Roles.First(), user.Roles.First().Name);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.Login == login)), Times.Once);
            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) => filter.UserID == user.ID)), Times.Once);
        }
    }
}
