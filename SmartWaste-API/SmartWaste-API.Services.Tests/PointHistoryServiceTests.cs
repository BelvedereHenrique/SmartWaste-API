using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Services.Interfaces;
using SmartWaste_API.Library.Tests;
using SmartWaste_API.Services.Security;
using SmartWaste_API.Business.Interfaces;
using Moq;
using System.Collections.Generic;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class PointHistoryServiceTests
    {
        [TestMethod]
        public void GetListForAnUnauthorizedUser()
        {
            var filter = new PointHistoryFilterContract();

            var identity = SecurityManagerHelper.GetUnauthenticatedIdentity();

            var service = (IPointHistoryService)new PointHistoryService(null, identity.Object);
            var result = service.GetList(filter);

            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void GetListForNotCompanyUser()
        {
            var filter = new PointHistoryFilterContract();
            var histories = new List<PointHistoryContract>();

            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.USER
            });

            var pointHistoryRepository = GetPointHistoryRepository();
            pointHistoryRepository.Setup(x => x.GetList(filter)).Returns(histories);

            var service = (IPointHistoryService)new PointHistoryService(pointHistoryRepository.Object, identity.Object);
            var result = service.GetList(filter);

            Assert.AreEqual(result, histories);

            pointHistoryRepository.Verify(x => x.GetList(It.Is((PointHistoryFilterContract f) =>
                f.PersonID == person.ID &&
                f.CompanyID == null
            )), Times.Once);
        }

        [TestMethod]
        public void GetListForCompanyUser()
        {
            var filter = new PointHistoryFilterContract();
            var histories = new List<PointHistoryContract>();

            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.COMPANY_USER
            });

            var pointHistoryRepository = GetPointHistoryRepository();
            pointHistoryRepository.Setup(x => x.GetList(filter)).Returns(histories);

            var service = (IPointHistoryService)new PointHistoryService(pointHistoryRepository.Object, identity.Object);
            var result = service.GetList(filter);

            Assert.AreEqual(result, histories);

            pointHistoryRepository.Verify(x => x.GetList(It.Is((PointHistoryFilterContract f) =>
                f.PersonID == null &&
                f.CompanyID == person.CompanyID
            )), Times.Once);
        }

        private Mock<IPointHistoryRepository> GetPointHistoryRepository()
        {
            return new Mock<IPointHistoryRepository>();
        }
    }
}
