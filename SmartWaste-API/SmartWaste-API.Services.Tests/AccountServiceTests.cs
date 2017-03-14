using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWaste_API.Services.Interfaces;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using Moq;
using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Person;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void AddEnterpriseTest_Success()
        {
            var enterpriseId = Guid.NewGuid();
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(false);
            repo.Setup(x => x.GetUserEnterprise(It.IsAny<Guid>())).Returns(new AccountEnterpriseContract());
            repo.Setup(x => x.AddEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(enterpriseId);
            var user = MockAuthenticatedUser(true);

            var service = GetAccountService(repo.Object, null, null, user.Object);
            var result = service.AddEnterprise(It.IsAny<AccountEnterpriseContract>());

            Assert.AreEqual(result, enterpriseId);
            repo.Verify(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>()), Times.Exactly(1));
            repo.Verify(x => x.GetUserEnterprise(It.IsAny<Guid>()), Times.Exactly(1));
            repo.Verify(x => x.AddEnterprise(It.IsAny<AccountEnterpriseContract>()), Times.Exactly(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEnterpriseTest_AlreadyExists()
        {
            var enterpriseId = Guid.NewGuid();
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(true);
            var user = MockAuthenticatedUser(true);

            var service = GetAccountService(repo.Object, null, null, user.Object);
            var result = service.AddEnterprise(It.IsAny<AccountEnterpriseContract>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEnterpriseTest_UserHasAnEnterprise()
        {
            var enterpriseId = Guid.NewGuid();
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(false);
            repo.Setup(x => x.GetUserEnterprise(It.IsAny<Guid>())).Returns(new AccountEnterpriseContract() { ID = Guid.NewGuid()});
            repo.Setup(x => x.AddEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(enterpriseId);
            var user = MockAuthenticatedUser(true);

            var service = GetAccountService(repo.Object, null, null, user.Object);
            var result = service.AddEnterprise(It.IsAny<AccountEnterpriseContract>());
        }

        [TestMethod]
        public void DoChangesToNewEnterpriseTest_UserAuthenticated()
        {
            var user = MockAuthenticatedUser(true);
            var enterpriseId = Guid.NewGuid();
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(false);
            repo.Setup(x => x.GetUserEnterprise(It.IsAny<Guid>())).Returns(new AccountEnterpriseContract());
            repo.Setup(x => x.AddEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(enterpriseId);

            var personService = new Mock<IPersonService>();
            personService.Setup(x => x.SetCompanyID(It.IsAny<Guid>(), It.IsAny<PersonFilterContract>()));

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.SetUserRoles(It.IsAny<Guid>(), It.IsAny<List<Guid>>()));

            //TODO: Implement RestartPassword Logic

            var service = GetAccountService(repo.Object,personService.Object,userService.Object, user.Object);
            var result = service.DoChangesToNewEnterprise(It.IsAny<AccountEnterpriseContract>());

            Assert.AreEqual(result, enterpriseId);
            repo.Verify(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>()), Times.Exactly(1));
            repo.Verify(x => x.GetUserEnterprise(It.IsAny<Guid>()), Times.Exactly(1));
            repo.Verify(x => x.AddEnterprise(It.IsAny<AccountEnterpriseContract>()), Times.Exactly(1));
            personService.Verify(x => x.SetCompanyID(It.IsAny<Guid>(), It.IsAny<PersonFilterContract>()),Times.Exactly(1));
            userService.Verify(x => x.SetUserRoles(It.IsAny<Guid>(), It.IsAny<List<Guid>>()),Times.Exactly(1));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DoChangesToNewEnterpriseTest_UserNotAuthenticated()
        {
            var user = MockAuthenticatedUser(false);
            var service = GetAccountService(null, null, null, user.Object);
            service.DoChangesToNewEnterprise(It.IsAny<AccountEnterpriseContract>());
        }
        
        [TestMethod]
        public void CheckEnterpriseTest()
        {
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>())).Returns(true);

            var service = GetAccountService(repo.Object, null, null, null);
            var result = service.CheckEnterprise(It.IsAny<AccountEnterpriseContract>());

            Assert.AreEqual(result, true);
            repo.Verify(x => x.CheckEnterprise(It.IsAny<AccountEnterpriseContract>()),Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserEnterprise_AuthenticatedUser()
        {
            var user = MockAuthenticatedUser(true);
            var enterprise = new AccountEnterpriseContract();
            var repo = new Mock<IAccountRepository>();
            repo.Setup(x => x.GetUserEnterprise(It.IsAny<Guid>())).Returns(enterprise);

            var service = GetAccountService(repo.Object, null, null, user.Object);
            var result = service.GetUserEnterprise();
            Assert.AreEqual(result, enterprise);
            repo.Verify(x => x.GetUserEnterprise(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GetUserEnterprise_NotAuthenticatedUser()
        {
            var user = MockAuthenticatedUser(false);
            var service = GetAccountService(null, null, null, user.Object);
            service.GetUserEnterprise();
        }


        internal IAccountService GetAccountService (IAccountRepository _accRepo, IPersonService _pService, IUserService _uService, ISecurityManager<IdentityContract> user)
        {
            return (IAccountService)new AccountService(_accRepo, _pService, _uService, user);
        }
        internal Mock<ISecurityManager<IdentityContract>> MockAuthenticatedUser(bool isAuthenticated)
        {
            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            var identity = new IdentityContract() { IsAuthenticated = isAuthenticated };
            if (isAuthenticated) { identity.User = new SmarteWaste_API.Contracts.User.UserContract() { ID = Guid.NewGuid() }; }
            securityManager.Setup(x => x.User).Returns(identity);
            return securityManager;
        }
    }
}
