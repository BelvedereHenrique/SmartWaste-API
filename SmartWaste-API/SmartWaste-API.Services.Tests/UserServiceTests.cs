using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void GetUserServiceTest()
        {
            var user = new UserContract();
            var filter = new UserFilterContract();
            var repo = new Mock<IUserRepository>();
            repo.Setup(x => x.Get(filter)).Returns(user);

            var service = GetUserService(repo.Object);
            var result = service.Get(filter);

            Assert.AreEqual(result, user);
            repo.Verify(x => x.Get(filter), Times.Exactly(1));
        }

        [TestMethod]
        public void SetUserRolesTest()
        {
            Guid userID = Guid.NewGuid();
            var roles = new List<Guid>();

            var repo = new Mock<IUserRepository>();
            repo.Setup(x => x.SetUserRoles(userID,roles));

            var service = GetUserService(repo.Object);
            service.SetUserRoles(userID,roles);

            repo.Verify(x => x.SetUserRoles(userID, roles), Times.Exactly(1));
        }

        internal IUserService GetUserService(IUserRepository _userRepository)
        {
            return (IUserService)new UserService(_userRepository);
        }
    }
}
