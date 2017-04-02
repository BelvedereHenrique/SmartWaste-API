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
using SmartWaste_API.Library.Tests;
using System.Linq;

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
            pointService.Setup(x => x.GetPublicList(It.IsAny<PointFilterContract>())).Returns(points);

            var service = (IPointService)new PointService(pointService.Object, securityManager.Object, null, null);

            var result = service.GetList(new PointFilterContract());

            Assert.AreEqual(result, points);

            pointService.Verify(x => x.GetPublicList(It.IsAny<PointFilterContract>()), Times.Once);
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
            pointService.Setup(x => x.GetUserList(identity.Person.ID, It.IsAny<PointFilterContract>())).Returns(points);

            var service = (IPointService)new PointService(pointService.Object, securityManager.Object, null, null);

            var result = service.GetList(new PointFilterContract());

            Assert.AreEqual(result, points);

            pointService.Verify(x => x.GetUserList(identity.Person.ID, It.IsAny<PointFilterContract>()), Times.Once);
        }

        [TestMethod]
        public void SetAsFullSuccessTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var point = new PointDetailedContract() {
                ID = Guid.NewGuid(),
                Status = PointStatusEnum.Empty,
                PointRouteStatus = PointRouteStatusEnum.Free,
                Type = PointTypeEnum.User
            };

            var pointRepository = GetPointRepository();
            pointRepository.Setup(x => x.GetUserDetailed(person.ID, It.IsAny<PointFilterContract>())).Returns(point);
            pointRepository.Setup(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()))
                .Callback((PointContract editedPoint, List<PointHistoryContract> histories) => {
                    Assert.AreEqual(histories.Count, 1);
                    Assert.AreEqual(histories.First().Date.Date, DateTime.Now.Date);
                    Assert.AreEqual(histories.First().Person.ID, person.ID);
                    Assert.AreEqual(histories.First().PointID, point.ID);
                    Assert.AreEqual(histories.First().Status, PointStatusEnum.Full);
                    Assert.IsFalse(String.IsNullOrEmpty(histories.First().Reason));

                    Assert.AreEqual(editedPoint.Status, PointStatusEnum.Full);
                    Assert.AreEqual(editedPoint.PointRouteStatus, PointRouteStatusEnum.Free);
                    Assert.AreEqual(editedPoint.Type, PointTypeEnum.User);
                });

            var pointService = (IPointService)new PointService(pointRepository.Object, identity.Object, null, null);
            var result = pointService.SetAsFull();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);

            pointRepository.Verify(x => x.GetUserDetailed(person.ID, It.Is((PointFilterContract filter) =>
                filter.PersonID == person.ID
            )), Times.Once);
        }

        [TestMethod]
        public void SetAsFullWhenUserIsCompanyTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_USER
            });
            
            var pointRepository = GetPointRepository();
            pointRepository.Setup(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()));

            var pointService = (IPointService)new PointService(pointRepository.Object, identity.Object, null, null);
            var result = pointService.SetAsFull();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Messages.First().Message));

            pointRepository.Verify(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()), 
                Times.Never);
        }

        [TestMethod]
        public void SetAsFullWhenUserDoesntHavePointsTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var pointRepository = GetPointRepository();
            pointRepository.Setup(x => x.GetUserDetailed(person.ID, It.IsAny<PointFilterContract>()));
            pointRepository.Setup(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()));

            var pointService = (IPointService)new PointService(pointRepository.Object, identity.Object, null, null);
            var result = pointService.SetAsFull();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Messages.First().Message));

            pointRepository.Verify(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()),
                Times.Never);
        }

        [TestMethod]
        public void SetAsFullWhenPointIsAlreadyFullTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var point = new PointDetailedContract(){
                Status = PointStatusEnum.Full
            };

            var pointRepository = GetPointRepository();
            pointRepository.Setup(x => x.GetUserDetailed(person.ID, It.IsAny<PointFilterContract>())).Returns(point);
            pointRepository.Setup(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()));

            var pointService = (IPointService)new PointService(pointRepository.Object, identity.Object, null, null);
            var result = pointService.SetAsFull();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Messages.First().Message));

            pointRepository.Verify(x => x.Edit(It.IsAny<PointContract>(), It.IsAny<List<PointHistoryContract>>()),
                Times.Never);
        }        

        private Mock<IPointRepository> GetPointRepository()
        {
            return new Mock<IPointRepository>();
        }
    }
}