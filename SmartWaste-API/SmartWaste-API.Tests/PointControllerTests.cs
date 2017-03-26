using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Services.Interfaces;
using SmarteWaste_API.Contracts.Point;
using System.Collections.Generic;
using SmartWaste_API.Controllers;
using SmartWaste_API.Models;
using System.Web.Http.Results;
using System.Linq;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Library.Tests;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Contracts.Tests;
using SmarteWaste_API.Contracts.OperationResult;

namespace SmartWaste_API.Tests
{
    [TestClass]
    public class PointControllerTests
    {
        [TestMethod]
        public void GetListSuccessTest()
        {
            var filter = new PointFilterContract();
            var points = new List<PointContract>();

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Returns(points);

            var controller = new PointController(pointService.Object, null, null, null);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<List<PointContract>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, points);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
        }

        [TestMethod]
        public void GetListFailTest()
        {
            var filter = new PointFilterContract();
            var points = new List<PointContract>();

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Throws(new Exception());

            var controller = new PointController(pointService.Object, null, null, null);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(x => x.IsError && !String.IsNullOrEmpty(x.Message)));
        }

        [TestMethod]
        public void GetDetailedListSuccessTest()
        {
            var filter = new PointFilterContract();
            var points = new List<PointDetailedContract>();

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var controller = new PointController(pointService.Object, null, null, null);
            var jsonModel = controller.GetDetailedList(filter) as OkNegotiatedContentResult<JsonModel<List<PointDetailedContract>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, points);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
        }

        [TestMethod]
        public void GetDetailedListFailTest()
        {
            var filter = new PointFilterContract();            

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Throws(new Exception());

            var controller = new PointController(pointService.Object, null, null, null);
            var jsonModel = controller.GetDetailedList(filter) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(x => x.IsError && !String.IsNullOrEmpty(x.Message)));
        }

        [TestMethod]
        public void GetPeopleFromCompanySuccessfulTest()
        {
            var persons = new List<PersonContract>();

            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>());
            
            var personService = GetPersonService();
            personService.Setup(x => x.GetList(It.IsAny<PersonFilterContract>())).Returns(persons);

            var controller = new PointController(null, personService.Object, securityManager.Object, null);
            var jsonModel = controller.GetPeopleFromCompany() as OkNegotiatedContentResult<JsonModel<List<PersonContract>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, persons);

            personService.Verify(x => x.GetList(It.IsAny<PersonFilterContract>()), Times.Once);
            personService.Verify(x => x.GetList(It.Is((PersonFilterContract filter) => filter.CompanyID == person.CompanyID)), Times.Once);
        }

        [TestMethod]
        public void GetPeopleFromCompanyFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>());

            var personService = GetPersonService();
            personService.Setup(x => x.GetList(It.IsAny<PersonFilterContract>())).Throws(new Exception());

            var controller = new PointController(null, personService.Object, securityManager.Object, null);
            var jsonModel = controller.GetPeopleFromCompany() as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.AreEqual(jsonModel.Content.Result, false);

            personService.Verify(x => x.GetList(It.IsAny<PersonFilterContract>()), Times.Once);
            personService.Verify(x => x.GetList(It.Is((PersonFilterContract filter) => filter.CompanyID == person.CompanyID)), Times.Once);
        }

        [TestMethod]
        public void SetAsFullSuccessTest()
        {
            var operationResult = OperationResultHelper.GetSuccess(3);

            var pointService = GetPointService();
            pointService.Setup(x => x.SetAsFull()).Returns(operationResult);

            var controller = new PointController(pointService.Object, null, null, null);
            var result = (controller.SetAsFull() as OkNegotiatedContentResult<JsonModel<OperationResult>>).Content;

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.IsTrue(result.Result.Success);
        }

        [TestMethod]
        public void SetAsFullFailTest()
        {
            var pointService = GetPointService();
            pointService.Setup(x => x.SetAsFull()).Throws(new Exception());

            var controller = new PointController(pointService.Object, null, null, null);
            var result = (controller.SetAsFull() as OkNegotiatedContentResult<JsonModel<bool>>).Content;

            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.Result);
            Assert.AreEqual(result.Messages.Count, 1);
        }

        [TestMethod]
        public void GetDetailedSuccessTest()
        {
            var filter = new PointFilterContract();

            var point = new PointDetailedContract();
            var histories = new List<PointHistoryContract>() {
                new PointHistoryContract()
            };

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailed(filter)).Returns(point);

            var pointHistoryService = GetPointHistoryService();
            pointHistoryService.Setup(x => x.GetList(It.IsAny<PointHistoryFilterContract>())).Returns(histories);

            var controller = new PointController(pointService.Object, null, null, pointHistoryService.Object);
            var result = (controller.GetDetailed(filter) as OkNegotiatedContentResult<JsonModel<PointDetailedHistoriesModel>>).Content;

            Assert.IsTrue(result.Success);            
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.AreEqual(result.Result.Histories, histories);
        }

        [TestMethod]
        public void GetDetailedWhenPointIsNullTest()
        {
            var filter = new PointFilterContract();           

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailed(filter));

            var controller = new PointController(pointService.Object, null, null, null);
            var result = (controller.GetDetailed(filter) as OkNegotiatedContentResult<JsonModel<bool>>).Content;

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsFalse(result.Result);
        }

        private Mock<IPointService> GetPointService()
        {
            return new Mock<IPointService>();
        }

        private Mock<IPersonService> GetPersonService()
        {
            return new Mock<IPersonService>();
        }

        private Mock<IPointHistoryService> GetPointHistoryService()
        {
            return new Mock<IPointHistoryService>();
        }
    }
}