using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Services.Interfaces;
using SmarteWaste_API.Contracts.Route;
using System.Linq;
using SmartWaste_API.Controllers;
using System.Web.Http.Results;
using SmartWaste_API.Models;
using SmartWaste_API.Contracts.Tests;
using SmarteWaste_API.Contracts.OperationResult;

namespace SmartWaste_API.Tests
{    
    [TestClass]
    public class RouteControllerTests
    {    
        [TestMethod]
        public void GetSuccefullTest()
        {
            var routeContract = GetRoutes().First();
            var filter = GetRouteFilterContract();

            var routeService = GetRouteService();
            routeService.Setup(x => x.Get(filter)).Returns(routeContract);

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Get(filter) as OkNegotiatedContentResult<JsonModel<RouteContract>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, routeContract);

            routeService.Verify(x => x.Get(It.IsAny<RouteFilterContract>()), Times.Once);
            routeService.Verify(x => x.Get(filter), Times.Once);
        }

        [TestMethod]
        public void GetFailTest()
        {            
            var filter = GetRouteFilterContract();

            var routeService = GetRouteService();
            routeService.Setup(x => x.Get(filter)).Throws(new Exception());

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Get(filter) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(m => !String.IsNullOrWhiteSpace(m.Message) && m.IsError));
            Assert.AreEqual(jsonModel.Content.Result, false);

            routeService.Verify(x => x.Get(It.IsAny<RouteFilterContract>()), Times.Once);
            routeService.Verify(x => x.Get(filter), Times.Once);
        }

        [TestMethod]
        public void GetListSuccefullTest()
        {
            var routeContracts = GetRoutes();
            var filter = GetRouteFilterContract();

            var routeService = GetRouteService();
            routeService.Setup(x => x.GetList(filter)).Returns(routeContracts);

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<List<RouteContract>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, routeContracts);

            routeService.Verify(x => x.GetList(It.IsAny<RouteFilterContract>()), Times.Once);
            routeService.Verify(x => x.GetList(filter), Times.Once);
        }

        [TestMethod]
        public void GetListFailTest()
        {
            var filter = GetRouteFilterContract();

            var routeService = GetRouteService();
            routeService.Setup(x => x.GetList(filter)).Throws(new Exception());

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(m => !String.IsNullOrWhiteSpace(m.Message) && m.IsError));
            Assert.AreEqual(jsonModel.Content.Result, false);

            routeService.Verify(x => x.GetList(It.IsAny<RouteFilterContract>()), Times.Once);
            routeService.Verify(x => x.GetList(filter), Times.Once);
        }

        [TestMethod]
        public void CreateSuccefullTest()
        {
            var routeAddModel = GetRouteAddModel();
            var succefullResult = OperationResultHelper.GetSuccess<Guid>(Guid.NewGuid(), 3);
            
            var routeService = GetRouteService();
            routeService.Setup(x => x.Create(routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes)).Returns(succefullResult);

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Create(routeAddModel) as OkNegotiatedContentResult<JsonModel<OperationResult<Guid>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, succefullResult);

            routeService.Verify(x => x.Create(It.IsAny<Guid?>(), It.IsAny<List<Guid>>(), It.IsAny<Decimal>(), It.IsAny<Decimal>()), Times.Once);
            routeService.Verify(x => x.Create(routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes), Times.Once);
        }

        [TestMethod]
        public void CreateFailTest()
        {
            var routeAddModel = GetRouteAddModel();

            var routeService = GetRouteService();
            routeService.Setup(x => x.Create(routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes)).Throws(new Exception());

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Create(routeAddModel) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(m => !String.IsNullOrWhiteSpace(m.Message) && m.IsError));
            Assert.AreEqual(jsonModel.Content.Result, false);

            routeService.Verify(x => x.Create(It.IsAny<Guid?>(), It.IsAny<List<Guid>>(), It.IsAny<Decimal>(), It.IsAny<Decimal>()), Times.Once);
            routeService.Verify(x => x.Create(routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes), Times.Once);
        }

        [TestMethod]
        public void RecreateSuccefullTest()
        {
            var routeAddModel = GetRouteAddModel();
            var succefullResult = OperationResultHelper.GetSuccess<Guid>(Guid.NewGuid(), 3);

            var routeService = GetRouteService();
            routeService.Setup(x => x.Recreate(routeAddModel.RouteID.Value, routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes)).Returns(succefullResult);

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Recreate(routeAddModel) as OkNegotiatedContentResult<JsonModel<OperationResult<Guid>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, succefullResult);

            routeService.Verify(x => x.Recreate(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<List<Guid>>(), It.IsAny<Decimal>(), It.IsAny<Decimal>()), Times.Once);
            routeService.Verify(x => x.Recreate(routeAddModel.RouteID.Value, routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes), Times.Once);
        }

        [TestMethod]
        public void RecreateFailTest()
        {
            var routeAddModel = GetRouteAddModel();
            
            var routeService = GetRouteService();
            routeService.Setup(x => x.Recreate(routeAddModel.RouteID.Value, routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes)).Throws(new Exception());

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Recreate(routeAddModel) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(m => !String.IsNullOrWhiteSpace(m.Message) && m.IsError));
            Assert.AreEqual(jsonModel.Content.Result, false);

            routeService.Verify(x => x.Recreate(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<List<Guid>>(), It.IsAny<Decimal>(), It.IsAny<Decimal>()), Times.Once);
            routeService.Verify(x => x.Recreate(routeAddModel.RouteID.Value, routeAddModel.AssignedToID, routeAddModel.PointIDs, routeAddModel.ExpectedKilometers, routeAddModel.ExpectedMinutes), Times.Once);
        }

        [TestMethod]
        public void DisableSuccefullTest()
        {
            var routeDisableModel = GetRouteDisableModel();
            var succefullResult = OperationResultHelper.GetSuccess(3);

            var routeService = GetRouteService();
            routeService.Setup(x => x.Disable(routeDisableModel.RouteID)).Returns(succefullResult);

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Disable(routeDisableModel) as OkNegotiatedContentResult<JsonModel<OperationResult>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            Assert.AreEqual(jsonModel.Content.Result, succefullResult);

            routeService.Verify(x => x.Disable(It.IsAny<Guid>()), Times.Once);
            routeService.Verify(x => x.Disable(routeDisableModel.RouteID), Times.Once);
        }

        [TestMethod]
        public void DisableFailTest()
        {
            var routeDisableModel = GetRouteDisableModel();

            var routeService = GetRouteService();
            routeService.Setup(x => x.Disable(routeDisableModel.RouteID)).Throws(new Exception());

            var controller = new RouteController(routeService.Object);
            var jsonModel = controller.Disable(routeDisableModel) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(m => !String.IsNullOrWhiteSpace(m.Message) && m.IsError));
            Assert.AreEqual(jsonModel.Content.Result, false);

            routeService.Verify(x => x.Disable(It.IsAny<Guid>()), Times.Once);
            routeService.Verify(x => x.Disable(routeDisableModel.RouteID), Times.Once);
        }

        private RouteDisableModel GetRouteDisableModel()
        {
            return new RouteDisableModel()
            {
                RouteID = Guid.NewGuid()
            };
        }

        private RouteAddModel GetRouteAddModel()
        {
            return new RouteAddModel() {
                AssignedToID = Guid.NewGuid(),
                ExpectedKilometers = 100.454M,
                ExpectedMinutes = 235.500M,
                PointIDs = new List<Guid>() {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                RouteID = Guid.NewGuid()
            };
        }

        private List<RouteContract> GetRoutes()
        {
            return new List<RouteContract>() {
                new RouteContract(){
                    ID = Guid.NewGuid()
                },
                new RouteContract() {
                    ID = Guid.NewGuid()
                }
            };
        }

        private RouteFilterContract GetRouteFilterContract()
        {
            return new RouteFilterContract()
            {
                AssignedToID = Guid.NewGuid(),
                CompanyID = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                ID = Guid.NewGuid(),
                LoadUnassigned = true,
                Status = RouteStatusEnum.Disabled
            }; 
        }

        private Mock<IRouteService> GetRouteService()
        {
            return new Mock<IRouteService>();
        }
    }
}
