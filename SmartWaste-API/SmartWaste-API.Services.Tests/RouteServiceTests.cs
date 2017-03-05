using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;
using SmartWaste_API.Contracts.Tests;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Tests;
using SmartWaste_API.Services.Security;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Point;
using System.Linq;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class RouteServiceTests
    {
        [TestMethod]
        public void CreateSuccessfulTest()
        {
            var securityManager = GetAuthenticatedSecurityManager();

            var totalWarningMessages = 3;
            var route = GetRouteContract();
            var pointIDs = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var operationResult = OperationResultHelper.GetSuccess<RouteContract>(route, totalWarningMessages);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CanCreate(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes))
                .Returns(operationResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Create(operationResult.Result, operationResult.Result.Histories, operationResult.Result.Points));

            var routeService = (IRouteService)new RouteService(securityManager.Object, routeRepository.Object, routeValidationService.Object);

            var result = routeService.Create(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, totalWarningMessages);
            Assert.AreEqual(result.Result, route.ID);

            routeValidationService.Verify(x => x.CanCreate(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes), Times.Once);
            routeRepository.Verify(x => x.Create(operationResult.Result, operationResult.Result.Histories, operationResult.Result.Points), Times.Once);
        }

        [TestMethod]
        public void CreateFailTest()
        {
            var securityManager = GetAuthenticatedSecurityManager();

            var totalErrorMessages = 3;
            var route = GetRouteContract();
            var pointIDs = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var operationResult = OperationResultHelper.GetFail<RouteContract>(route, totalErrorMessages);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CanCreate(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes))
                .Returns(operationResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Create(operationResult.Result, operationResult.Result.Histories, operationResult.Result.Points));

            var routeService = (IRouteService)new RouteService(securityManager.Object, routeRepository.Object, routeValidationService.Object);

            var result = routeService.Create(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, totalErrorMessages);
            Assert.AreEqual(result.Result, Guid.Empty);

            routeValidationService.Verify(x => x.CanCreate(route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes), Times.Once);
            routeRepository.Verify(x => x.Create(It.IsAny<RouteContract>(), It.IsAny<List<RouteHistoryContract>>(), It.IsAny<List<PointDetailedContract>>()), Times.Never);
        }

        [TestMethod]
        public void GetWithoutValidationErrors()
        {
            var routeContract = GetRouteContract();

            var filter = new RouteFilterContract();
            var checkedFilter = new RouteFilterContract();
            var filterResult = OperationResultHelper.GetSuccess<RouteFilterContract>(checkedFilter, 2);
                        
            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CheckFilter(filter)).Returns(filterResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Get(checkedFilter)).Returns(routeContract);

            var routeService = (IRouteService)new RouteService(null, routeRepository.Object, routeValidationService.Object);
            var routeResult = routeService.Get(filter);

            Assert.AreEqual(routeResult, routeContract);

            routeValidationService.Verify(x => x.CheckFilter(filter), Times.Once);
            routeRepository.Verify(x => x.Get(checkedFilter), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetWithValidationErrors()
        {
            var routeContract = GetRouteContract();

            var filter = new RouteFilterContract();
            var checkedFilter = new RouteFilterContract();
            var filterResult = OperationResultHelper.GetFail<RouteFilterContract>(checkedFilter, 2);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CheckFilter(filter)).Returns(filterResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Get(checkedFilter)).Returns(routeContract);

            var routeService = (IRouteService)new RouteService(null, null, routeValidationService.Object);
            var routeResult = routeService.Get(filter);
        }

        [TestMethod]
        public void GetListWithoutValidationErrors()
        {
            var routeContracts = GetRouteContracts();
            
            var filter = new RouteFilterContract();
            var checkedFilter = new RouteFilterContract();
            var filterResult = OperationResultHelper.GetSuccess<RouteFilterContract>(checkedFilter, 2);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CheckFilter(filter)).Returns(filterResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.GetList(checkedFilter)).Returns(routeContracts);

            var routeService = (IRouteService)new RouteService(null, routeRepository.Object, routeValidationService.Object);
            var routeResult = routeService.GetList(filter);

            Assert.AreEqual(routeResult, routeContracts);

            routeValidationService.Verify(x => x.CheckFilter(filter), Times.Once);
            routeRepository.Verify(x => x.GetList(checkedFilter), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetListWithValidationErrors()
        {
            var routeContracts = GetRouteContracts();

            var filter = new RouteFilterContract();
            var checkedFilter = new RouteFilterContract();
            var filterResult = OperationResultHelper.GetFail<RouteFilterContract>(checkedFilter, 2);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CheckFilter(filter)).Returns(filterResult);

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.GetList(checkedFilter)).Returns(routeContracts);

            var routeService = (IRouteService)new RouteService(null, null, routeValidationService.Object);
            var routeResult = routeService.GetList(filter);
        }

        [TestMethod]
        public void DisableWithoutValidationErrors()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() { });

            var route = GetRouteContract();
            var routeResult = GetRouteContract();
            var checkFilterResult = new RouteFilterContract();

            var totalWarningMessages = 2;
            var disableResult = OperationResultHelper.GetSuccess<RouteContract>(routeResult, totalWarningMessages);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CanDisable(route)).Returns(disableResult);
            routeValidationService.Setup(x => x.CheckFilter(It.IsAny<RouteFilterContract>())).Returns(OperationResultHelper.GetSuccess<RouteFilterContract>(checkFilterResult, 0));

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Edit(disableResult.Result, It.IsAny<List<RouteHistoryContract>>(), disableResult.Result.Points));
            routeRepository.Setup(x => x.Get(It.IsAny<RouteFilterContract>())).Returns(route);

            var routeService = (IRouteService)new RouteService(securityManager.Object, routeRepository.Object, routeValidationService.Object);
            var serviceResult = routeService.Disable(route.ID);

            Assert.IsTrue(serviceResult.Success);
            Assert.AreEqual(serviceResult.Messages.Count, totalWarningMessages);

            routeRepository.Verify(x => x.Edit(disableResult.Result, It.Is((List<RouteHistoryContract> histories) => 
                histories.Count == 1 &&
                ValidateDisableHistory(histories.First(), routeResult.ID, person.ID)
                ), disableResult.Result.Points), Times.Once);

            routeValidationService.Verify(x => x.CanDisable(route), Times.Once);
            routeValidationService.Verify(x => x.CheckFilter(It.Is((RouteFilterContract filter) => 
                filter.ID == route.ID
            )), Times.Once);

            routeRepository.Verify(x => x.Get(checkFilterResult), Times.Once);
        }

        [TestMethod]
        public void DisableWithValidationErrors()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() { });

            var route = GetRouteContract();
            var routeResult = GetRouteContract();
            var checkFilterResult = new RouteFilterContract();

            var totalErrorMessages = 2;
            var disableResult = OperationResultHelper.GetFail<RouteContract>(routeResult, totalErrorMessages);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.CanDisable(route)).Returns(disableResult);
            routeValidationService.Setup(x => x.CheckFilter(It.IsAny<RouteFilterContract>())).Returns(OperationResultHelper.GetSuccess<RouteFilterContract>(checkFilterResult, 0));

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Get(It.IsAny<RouteFilterContract>())).Returns(route);

            var routeService = (IRouteService)new RouteService(securityManager.Object, routeRepository.Object, routeValidationService.Object);
            var serviceResult = routeService.Disable(route.ID);

            Assert.IsFalse(serviceResult.Success);
            Assert.AreEqual(serviceResult.Messages.Count, totalErrorMessages);

            routeRepository.Verify(x => x.Edit(It.IsAny<RouteContract>(), It.IsAny<List<RouteHistoryContract>>(), It.IsAny<List<PointDetailedContract>>()), Times.Never);
        }

        [TestMethod]
        public void RecreateWithoutValidationErrors()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() { });

            var pointIDs = new List<Guid>() {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            var sharedPoint = new PointDetailedContract() {
                ID = Guid.NewGuid()
            };

            var oldRoute = GetRouteContract();
            var oldRouteDisableResult = GetRouteContract();
            oldRouteDisableResult.Points = new List<PointDetailedContract>() {
                sharedPoint,
                new PointDetailedContract() { ID = Guid.NewGuid() }
            };

            var route = GetRouteContract();

            var routeResult = GetRouteContract();
            var recreatedRoute = GetRouteContract();
            recreatedRoute.Histories = new List<RouteHistoryContract>() {
                new RouteHistoryContract()
            };
            recreatedRoute.Points = new List<PointDetailedContract>() {
                sharedPoint,
                new PointDetailedContract() { ID = Guid.NewGuid() }
            };

            var checkFilterResult = new RouteFilterContract();
            var canReacreateResult = OperationResultHelper.GetSuccess<RouteContract>(recreatedRoute, 2);

            var totalErrorMessages = 2;
            var disableResult = OperationResultHelper.GetFail<RouteContract>(routeResult, totalErrorMessages);

            var routeValidationService = GetRouteValidationService();
            routeValidationService.Setup(x => x.Disable(route)).Returns(oldRouteDisableResult);
            routeValidationService.Setup(x => x.CanRecreate(route, route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes)).Returns(canReacreateResult);
            routeValidationService.Setup(x => x.CanDisable(route)).Returns(disableResult);
            routeValidationService.Setup(x => x.CheckFilter(It.IsAny<RouteFilterContract>())).Returns(OperationResultHelper.GetSuccess<RouteFilterContract>(checkFilterResult, 0));

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Get(It.IsAny<RouteFilterContract>())).Returns(route);

            var routeService = (IRouteService)new RouteService(securityManager.Object, routeRepository.Object, routeValidationService.Object);
            var serviceResult = routeService.Recreate(oldRoute.ID, route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes);

            Assert.IsTrue(serviceResult.Success);
            Assert.AreEqual(serviceResult.Messages.Count, totalErrorMessages);

            routeRepository.Verify(x => x.Recreate(oldRouteDisableResult, recreatedRoute, It.Is((List<RouteHistoryContract> histories) =>
                histories.Count == 2 &&
                ValidateDisableHistory(histories.Where(h => h.Status == RouteStatusEnum.Disabled).Single(), oldRouteDisableResult.ID, person.ID)
            ), It.Is((List<PointDetailedContract> points) =>
                points.Count == 3 &&
                points.Count(p => p.ID == oldRouteDisableResult.Points.Last().ID) == 1 &&
                points.Count(p => p.ID == recreatedRoute.Points.Last().ID) == 1 &&
                points.Count(p => p.ID == sharedPoint.ID) == 1
            )), Times.Once);
        }

        [TestMethod]
        public void RecreateWhenHasValidationErrors()
        {
            var route = GetRouteContract();
            var pointIDs = new List<Guid>() {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            var checkedFilter = new RouteFilterContract();

            var canRecreateResult = OperationResultHelper.GetFail<RouteContract>(route, 2);
            var checkFilterResult = OperationResultHelper.GetSuccess<RouteFilterContract>(checkedFilter, 2);

            var routeValidationService = GetRouteValidationService();            
            routeValidationService.Setup(x => x.CanRecreate(route, route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes)).Returns(canRecreateResult);            
            routeValidationService.Setup(x => x.CheckFilter(It.IsAny<RouteFilterContract>())).Returns(OperationResultHelper.GetSuccess<RouteFilterContract>(checkedFilter, 0));

            var routeRepository = GetRouteRepository();
            routeRepository.Setup(x => x.Get(checkedFilter)).Returns(route);

            var routeService = (IRouteService)new RouteService(null, routeRepository.Object, routeValidationService.Object);
            var result = routeService.Recreate(route.ID, route.AssignedTo.ID, pointIDs, route.ExpectedKilometers, route.ExpectedMinutes);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 2);
        }

        private bool ValidateDisableHistory(RouteHistoryContract history, Guid routeID, Guid personID)
        {
            if (history == null)
                return false;

            return history.RouteID == routeID &&
                   history.Status == RouteStatusEnum.Disabled &&
                   history.ID != Guid.Empty &&
                   history.PersonID == personID;
        }

        private RouteContract GetRouteContract()
        {
            return new RouteContract()
            {
                AssignedTo = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = Guid.NewGuid()
                },
                ClosedOn = null,
                CompanyID = Guid.NewGuid(),
                CreatedBy = new SmarteWaste_API.Contracts.Person.PersonContract() { },
                CreatedOn = DateTime.Now,
                ExpectedKilometers = 4.5M,
                ExpectedMinutes = 0.5M,
                Histories = new List<RouteHistoryContract>() { },
                ID = Guid.NewGuid(),
                Points = new List<SmarteWaste_API.Contracts.Point.PointDetailedContract>() { },
                Status = RouteStatusEnum.Opened
            };
        }

        private List<RouteContract> GetRouteContracts()
        {
            return new List<RouteContract>() {
                GetRouteContract(),
                GetRouteContract()
            };
        }

        private Mock<ISecurityManager<IdentityContract>> GetAuthenticatedSecurityManager()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            return SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string> { RolesName.COMPANY_ROUTE });
        }

        private Mock<IRouteRepository> GetRouteRepository()
        {
            return new Mock<IRouteRepository>();
        }

        private Mock<IRouteValidationService> GetRouteValidationService()
        {
            return new Mock<IRouteValidationService>();
        }

    }
}
