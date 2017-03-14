using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Interfaces;
using SmarteWaste_API.Contracts.Point;
using System.Collections.Generic;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Security;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class PointServiceTest
    {
        [TestMethod]
        public void GetListWhenUserIsntAuthenticated()
        {
            var points = new List<PointContract>() {
                new PointContract()
            };

            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            securityManager.Setup(x => x.User).Returns(new IdentityContract() { IsAuthenticated = false });

            var pointService = new Mock<IPointRepository>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Returns(points);

            var service = (IPointService)new PointService(pointService.Object, securityManager.Object);

            var result = service.GetList(new PointFilterContract());

            Assert.AreEqual(result, points);

            pointService.Verify(x => x.GetList(It.Is((PointFilterContract filter) =>
                filter.Type == PointTypeEnum.CompanyTrashCan &&
                filter.Status == null &&
                filter.PersonID == null                
            )), Times.Once);
        }

        [TestMethod]
        public void GetListWhenUserIsntCompany()
        {
            var points = new List<PointContract>() {
                new PointContract()
            };

            var identity = new IdentityContract()
            {
                IsAuthenticated = true,
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = Guid.NewGuid()
                }
            };

            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            securityManager.Setup(x => x.User).Returns(identity);
            securityManager.Setup(x => x.IsInRole(RolesName.USER)).Returns(true);

            var pointService = new Mock<IPointRepository>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Returns(points);

            var service = (IPointService)new PointService(pointService.Object, securityManager.Object);

            var result = service.GetList(new PointFilterContract());

            Assert.AreEqual(result, points);

            pointService.Verify(x => x.GetList(It.Is((PointFilterContract filter) =>
                filter.Type == null &&
                filter.Status == null &&
                filter.PersonID == identity.Person.ID
            )), Times.Once);
        }

        [TestMethod]
        public void GetListWhenLatitudeAndLongitudeAreNull()
        {
            var points = new List<PointContract>() {
                new PointContract()
            };

            var identity = new IdentityContract()
            {
                IsAuthenticated = true,
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = Guid.NewGuid()
                }
            };

            var securityManager = new Mock<ISecurityManager<IdentityContract>>();
            securityManager.Setup(x => x.User).Returns(identity);
            securityManager.Setup(x => x.IsInRole(RolesName.USER)).Returns(true);

            var pointService = new Mock<IPointRepository>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Returns(points);

            var service = (IPointService)new PointService(pointService.Object, securityManager.Object);

            var result = service.GetList(new PointFilterContract());

            Assert.AreEqual(result, points);

            pointService.Verify(x => x.GetList(It.Is((PointFilterContract filter) =>
                filter.Type == null &&
                filter.Status == null &&
                filter.PersonID == identity.Person.ID
            )), Times.Once);
        }
    }
}