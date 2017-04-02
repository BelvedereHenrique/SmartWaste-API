using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWaste_API.Library.Tests;
using SmartWaste_API.Services.Security;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SmarteWaste_API.Contracts.Route;
using Moq;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.User;
using SmarteWaste_API.Contracts.Point;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class RouteValidationServiceTests
    {
        [TestMethod]
        public void IsUserAuthorizedToGetRoutesSuccessTest()
        {
            foreach (var role in new List<string>() { RolesName.COMPANY_USER, RolesName.COMPANY_ADMIN, RolesName.COMPANY_ROUTE })
            {
                var person = SecurityManagerHelper.GetPersonContract(true);
                var user = SecurityManagerHelper.GetUserContract();
                var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                    role
                });

                var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

                var result = routeValidationService.IsUserAuthorizedToGetRoutes();

                Assert.IsTrue(result.Success);
                Assert.AreEqual(result.Messages.Count, 0);
            }
        }

        [TestMethod]
        public void IsUserAuthorizedToGetRoutesFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                    RolesName.USER
                });

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

            var result = routeValidationService.IsUserAuthorizedToGetRoutes();

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
        }

        [TestMethod]
        public void IsAuthorizedUserTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

            var result = routeValidationService.IsUserAuthorized();

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void IsAuthorizedUserWhenUserIsntCompanyUserTest()
        {
            foreach (var role in new List<string>() { RolesName.COMPANY_USER, RolesName.USER })
            {
                var person = SecurityManagerHelper.GetPersonContract(true);
                var user = SecurityManagerHelper.GetUserContract();
                var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                    role
                });

                var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

                var result = routeValidationService.IsUserAuthorized();

                Assert.IsFalse(result.Success);
                Assert.AreEqual(result.Messages.Count, 1);
                Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            }
        }

        [TestMethod]
        public void IsUsersCompanyTheSameOfTheRouteTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>());

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

            var result = routeValidationService.IsUsersCompanyTheSameOfTheRoute(new SmarteWaste_API.Contracts.Route.RouteDetailedContract()
            {
                CompanyID = person.CompanyID.Value
            });

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void IsUsersCompanyTheSameOfTheRouteFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>());

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, securityManager.Object, null);

            var result = routeValidationService.IsUsersCompanyTheSameOfTheRoute(new SmarteWaste_API.Contracts.Route.RouteDetailedContract()
            {
                CompanyID = Guid.NewGuid()
            });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
        }

        [TestMethod]
        public void IsRouteStatusAbleToDisableSuccessTest()
        {
            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.IsRouteStatusAbleToDisable(new RouteDetailedContract()
            {
                Status = RouteStatusEnum.Opened
            });

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void IsRouteStatusAbleToDisableFailTest()
        {
            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.IsRouteStatusAbleToDisable(new RouteDetailedContract()
            {
                Status = RouteStatusEnum.Closed
            });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));

            result = routeValidationService.IsRouteStatusAbleToDisable(new RouteDetailedContract()
            {
                Status = RouteStatusEnum.Disabled
            });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
        }

        [TestMethod]
        public void DisableRouteTest()
        {
            var route = new RouteDetailedContract()
            {
                Status = RouteStatusEnum.Opened,
                RoutePoints = new List<RoutePointContract>() {
                    new RoutePointContract() {
                        Point = new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                            PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.InARoute
                        }
                    },
                    new RoutePointContract() {
                        Point = new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                            PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.InARoute
                        }
                    }
                }
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var disabledRoute = routeValidationService.Disable(route);

            Assert.AreEqual(disabledRoute.Status, RouteStatusEnum.Disabled);
            Assert.IsTrue(disabledRoute.RoutePoints.All(p => p.Point.PointRouteStatus == PointRouteStatusEnum.Free));
        }

        [TestMethod]
        public void AreAllPointsFreeSuccessfulTest()
        {
            var points = new List<SmarteWaste_API.Contracts.Point.PointDetailedContract>() {
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.Free
                },
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.Free
                }
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.AreAllPointsFree(points);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void AreAllPointsFreeFailTest()
        {
            var points = new List<SmarteWaste_API.Contracts.Point.PointDetailedContract>() {
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.Free
                },
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    PointRouteStatus = SmarteWaste_API.Contracts.Point.PointRouteStatusEnum.InARoute
                }
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.AreAllPointsFree(points);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
        }

        [TestMethod]
        public void AreAllPointsFullSuccessfulTest()
        {
            var points = new List<SmarteWaste_API.Contracts.Point.PointDetailedContract>() {
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    Status = SmarteWaste_API.Contracts.Point.PointStatusEnum.Full
                },
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    Status = SmarteWaste_API.Contracts.Point.PointStatusEnum.Full
                }
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.AreAllPointsFull(points);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void AreAllPointsFullFailTest()
        {
            var points = new List<SmarteWaste_API.Contracts.Point.PointDetailedContract>() {
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    Status = SmarteWaste_API.Contracts.Point.PointStatusEnum.Full
                },
                new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    Status = SmarteWaste_API.Contracts.Point.PointStatusEnum.Empty
                }
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);

            var result = routeValidationService.AreAllPointsFull(points);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
        }

        [TestMethod]
        public void IsAssignedUserValidSuccessedTest()
        {
            var loggedPerson = SecurityManagerHelper.GetPersonContract(true);
            var loggedUser = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(loggedPerson, loggedUser, new List<string>() {
                RolesName.COMPANY_USER
            });

            var assignedTo = Guid.NewGuid();

            var personContract = new PersonContract()
            {
                CompanyID = loggedPerson.CompanyID,
                UserID = Guid.NewGuid()
            };

            var userContract = new UserContract()
            {
                Roles = new List<SmarteWaste_API.Contracts.Role.RoleContract>() {
                    new SmarteWaste_API.Contracts.Role.RoleContract() {
                        Name = RolesName.COMPANY_USER
                    }
                }
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(personContract);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(userContract);

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, null);
            var result = routeValidationService.IsAssignedUserValid(assignedTo);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) => filter.ID == assignedTo)), Times.Once);
            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.ID == personContract.UserID)), Times.Once);
        }

        [TestMethod]
        public void IsAssignedUserValidWhenCompanyDoesntMatchTest()
        {
            var loggedPerson = SecurityManagerHelper.GetPersonContract(true);
            var loggedUser = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(loggedPerson, loggedUser, new List<string>() {
                RolesName.COMPANY_USER
            });

            var assignedTo = Guid.NewGuid();

            var personContract = new PersonContract()
            {
                CompanyID = Guid.NewGuid(),
                UserID = Guid.NewGuid()
            };

            var userContract = new UserContract()
            {
                Roles = new List<SmarteWaste_API.Contracts.Role.RoleContract>() {
                    new SmarteWaste_API.Contracts.Role.RoleContract() {
                        Name = RolesName.COMPANY_USER
                    }
                }
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(personContract);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(userContract);

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, null);
            var result = routeValidationService.IsAssignedUserValid(assignedTo);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !string.IsNullOrWhiteSpace(x.Message)));

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) => filter.ID == assignedTo)), Times.Once);
            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) => filter.ID == personContract.UserID)), Times.Once);
        }

        [TestMethod]
        public void IsAssignedUserValidWhenIsNullTest()
        {
            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, null, null);
            var result = routeValidationService.IsAssignedUserValid(null);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
        }

        [TestMethod]
        public void CanCreateSuccessTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var assignedPerson = new PersonContract() {
                ID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                CompanyID = person.CompanyID
            };

            var assignedUser = new UserContract() {
                ID = assignedPerson.UserID
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(assignedPerson);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(assignedUser);
            
            var points = GetPointDetailedContracts();
            
            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var expectedKilometers = 45.45M;
            var expectedMinutes = 88.88M;

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, pointService.Object);

            var result = routeValidationService.CanCreate(assignedPerson.ID, points.Select(x => x.ID).ToList(), expectedKilometers, expectedMinutes);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);

            Assert.AreEqual(result.Result.Status, RouteStatusEnum.Opened);
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.Point.Status == PointStatusEnum.Full));
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.Point.PointRouteStatus == PointRouteStatusEnum.InARoute));
            Assert.AreEqual(result.Result.ExpectedKilometers, expectedKilometers);
            Assert.AreEqual(result.Result.ExpectedMinutes, expectedMinutes);
            Assert.AreEqual(result.Result.CreatedOn.Date, DateTime.Now.Date);
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.IsCollected == null && x.CollectedOn == null && x.CollectedBy == null && x.Reason == null && x.ID != null && x.ID != Guid.Empty));
            Assert.IsNull(result.Result.ClosedOn);
            Assert.IsNull(result.Result.NavigationFinishedOn);
            Assert.IsNull(result.Result.NavigationFinishedOn);
            Assert.AreEqual(result.Result.AssignedTo.ID, assignedPerson.ID);
            Assert.AreEqual(result.Result.CompanyID, person.CompanyID);
            Assert.AreEqual(result.Result.CreatedBy.ID, person.ID);
            Assert.AreEqual(result.Result.Histories.Count, 1);
            Assert.AreEqual(result.Result.Histories.First().Person.ID, person.ID);
            Assert.AreEqual(result.Result.Histories.First().RouteID, result.Result.ID);
            Assert.AreEqual(result.Result.Histories.First().Status, RouteStatusEnum.Opened);
            Assert.AreEqual(result.Result.Histories.First().Date.Date, DateTime.Now.Date);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Result.Histories.First().Reason));

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) =>
                filter.ID == assignedPerson.ID
            )), Times.Once);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) =>
                filter.ID == assignedUser.ID
            )), Times.Once);

            pointService.Verify(x => x.GetDetailedList(It.Is((PointFilterContract filter) =>
                filter.IDs.Count == points.Count &&
                filter.IDs.Any(id => points.Any(p => p.ID == id))
            )), Times.Once);
        }

        [TestMethod]
        public void CanCreateFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var assignedPerson = new PersonContract()
            {
                ID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                CompanyID = person.CompanyID
            };

            var assignedUser = new UserContract()
            {
                ID = assignedPerson.UserID
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(assignedPerson);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(assignedUser);

            var points = GetPointDetailedContracts();
            points.First().Status = PointStatusEnum.Empty;
            points.Last().PointRouteStatus = PointRouteStatusEnum.InARoute;

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var expectedKilometers = 45.45M;
            var expectedMinutes = 88.88M;

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, pointService.Object);

            var result = routeValidationService.CanCreate(assignedPerson.ID, points.Select(x => x.ID).ToList(), expectedKilometers, expectedMinutes);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 4);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.IsNull(result.Result);

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) =>
                filter.ID == assignedPerson.ID
            )), Times.Once);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) =>
                filter.ID == assignedUser.ID
            )), Times.Once);

            pointService.Verify(x => x.GetDetailedList(It.Is((PointFilterContract filter) =>
                filter.IDs.Count == points.Count &&
                filter.IDs.Any(id => points.Any(p => p.ID == id))
            )), Times.Once);
        }

        [TestMethod]
        public void CanRecreateSuccessTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var assignedPerson = new PersonContract()
            {
                ID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                CompanyID = person.CompanyID
            };

            var assignedUser = new UserContract()
            {
                ID = assignedPerson.UserID
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(assignedPerson);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(assignedUser);

            var sharedPointID = Guid.NewGuid();

            var points = GetPointDetailedContracts();
            points.Add(new PointDetailedContract()
            {
                ID = sharedPointID,
                PointRouteStatus = PointRouteStatusEnum.InARoute,
                Status = PointStatusEnum.Full
            });
            
            var oldRoute = new RouteDetailedContract() {
                RoutePoints = new List<RoutePointContract>() {
                    new RoutePointContract() {
                        Point = new PointDetailedContract()
                        {
                            ID = sharedPointID,
                            PointRouteStatus = PointRouteStatusEnum.InARoute,
                            Status = PointStatusEnum.Full
                        }
                    }
                },
                Status = RouteStatusEnum.Opened,
                CompanyID = person.CompanyID.Value
            };

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var expectedKilometers = 45.45M;
            var expectedMinutes = 88.88M;

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, pointService.Object);

            var result = routeValidationService.CanRecreate(oldRoute, assignedPerson.ID, points.Select(x => x.ID).ToList(), expectedKilometers, expectedMinutes);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);

            Assert.AreEqual(result.Result.Status, RouteStatusEnum.Opened);
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.Point.Status == PointStatusEnum.Full));
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.Point.PointRouteStatus == PointRouteStatusEnum.InARoute));
            Assert.AreEqual(result.Result.ExpectedKilometers, expectedKilometers);
            Assert.AreEqual(result.Result.ExpectedMinutes, expectedMinutes);
            Assert.AreEqual(result.Result.CreatedOn.Date, DateTime.Now.Date);
            Assert.IsTrue(result.Result.RoutePoints.All(x => x.IsCollected == null && x.CollectedOn == null && x.CollectedBy == null && x.Reason == null && x.ID != null && x.ID != Guid.Empty));
            Assert.IsNull(result.Result.ClosedOn);
            Assert.IsNull(result.Result.NavigationFinishedOn);
            Assert.IsNull(result.Result.NavigationFinishedOn);
            Assert.AreEqual(result.Result.AssignedTo.ID, assignedPerson.ID);
            Assert.AreEqual(result.Result.CompanyID, person.CompanyID);
            Assert.AreEqual(result.Result.CreatedBy.ID, person.ID);
            Assert.AreEqual(result.Result.Histories.Count, 1);
            Assert.AreEqual(result.Result.Histories.First().Person.ID, person.ID);
            Assert.AreEqual(result.Result.Histories.First().RouteID, result.Result.ID);
            Assert.AreEqual(result.Result.Histories.First().Status, RouteStatusEnum.Opened);
            Assert.AreEqual(result.Result.Histories.First().Date.Date, DateTime.Now.Date);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Result.Histories.First().Reason));

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) =>
                filter.ID == assignedPerson.ID
            )), Times.Once);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) =>
                filter.ID == assignedUser.ID
            )), Times.Once);

            pointService.Verify(x => x.GetDetailedList(It.Is((PointFilterContract filter) =>
                filter.IDs.Count == points.Count &&
                filter.IDs.Any(id => points.Any(p => p.ID == id))
            )), Times.Once);
        }

        [TestMethod]
        public void CanRecreateFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var assignedPerson = new PersonContract()
            {
                ID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                CompanyID = person.CompanyID
            };

            var assignedUser = new UserContract()
            {
                ID = assignedPerson.UserID
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(assignedPerson);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(assignedUser);

            var sharedPointID = Guid.NewGuid();

            var points = GetPointDetailedContracts();
            points.First().PointRouteStatus = PointRouteStatusEnum.InARoute;
            points.Add(new PointDetailedContract()
            {
                ID = sharedPointID,
                PointRouteStatus = PointRouteStatusEnum.InARoute,
                Status = PointStatusEnum.Empty
            });

            var oldRoute = new RouteDetailedContract()
            {
                RoutePoints = new List<RoutePointContract>() {
                    new RoutePointContract() {
                        Point = new PointDetailedContract()
                        {
                            ID = sharedPointID,
                            PointRouteStatus = PointRouteStatusEnum.InARoute,
                            Status = PointStatusEnum.Full
                        }
                    }
                },
                Status = RouteStatusEnum.Opened,
                CompanyID = Guid.NewGuid()
            };

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var expectedKilometers = 45.45M;
            var expectedMinutes = 88.88M;

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, pointService.Object);

            var result = routeValidationService.CanRecreate(oldRoute, assignedPerson.ID, points.Select(x => x.ID).ToList(), expectedKilometers, expectedMinutes);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 4);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.IsNull(result.Result);

            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) =>
                filter.ID == assignedPerson.ID
            )), Times.Once);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) =>
                filter.ID == assignedUser.ID
            )), Times.Once);

            pointService.Verify(x => x.GetDetailedList(It.Is((PointFilterContract filter) =>
                filter.IDs.Count == points.Count &&
                filter.IDs.Any(id => points.Any(p => p.ID == id))
            )), Times.Once);
        }

        [TestMethod]
        public void CanRecreateWhenOldRouteCantBeDisabledTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var securityManager = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var assignedPerson = new PersonContract()
            {
                ID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                CompanyID = person.CompanyID
            };

            var assignedUser = new UserContract()
            {
                ID = assignedPerson.UserID
            };

            var personService = GetPersonService();
            personService.Setup(x => x.Get(It.IsAny<PersonFilterContract>())).Returns(assignedPerson);

            var userService = GetUserService();
            userService.Setup(x => x.Get(It.IsAny<UserFilterContract>())).Returns(assignedUser);

            var sharedPointID = Guid.NewGuid();

            var points = GetPointDetailedContracts();
            points.Add(new PointDetailedContract()
            {
                ID = sharedPointID,
                PointRouteStatus = PointRouteStatusEnum.InARoute,
                Status = PointStatusEnum.Full
            });

            var oldRoute = new RouteDetailedContract()
            {
                RoutePoints = new List<RoutePointContract>() {
                    new RoutePointContract() {
                        Point = new PointDetailedContract()
                        {
                            ID = sharedPointID,
                            PointRouteStatus = PointRouteStatusEnum.InARoute,
                            Status = PointStatusEnum.Full
                        }
                    }
                },
                Status = RouteStatusEnum.Closed,
                CompanyID = Guid.NewGuid()
            };

            var pointService = GetPointService();
            pointService.Setup(x => x.GetDetailedList(It.IsAny<PointFilterContract>())).Returns(points);

            var expectedKilometers = 45.45M;
            var expectedMinutes = 88.88M;

            var routeValidationService = (IRouteValidationService)new RouteValidationService(userService.Object, personService.Object, securityManager.Object, pointService.Object);

            var result = routeValidationService.CanRecreate(oldRoute, assignedPerson.ID, points.Select(x => x.ID).ToList(), expectedKilometers, expectedMinutes);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 2);
            Assert.IsTrue(result.Messages.All(x => x.IsError && !String.IsNullOrWhiteSpace(x.Message)));
            Assert.IsNotNull(result.Result);
            
            personService.Verify(x => x.Get(It.Is((PersonFilterContract filter) =>
                filter.ID == assignedPerson.ID
            )), Times.Once);

            userService.Verify(x => x.Get(It.Is((UserFilterContract filter) =>
                filter.ID == assignedUser.ID
            )), Times.Once);

            pointService.Verify(x => x.GetDetailedList(It.Is((PointFilterContract filter) =>
                filter.IDs.Count == points.Count &&
                filter.IDs.Any(id => points.Any(p => p.ID == id))
            )), Times.Once);
        }

        [TestMethod]
        public void GetFilterForOpenedRoutesSuccefullTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null,null, identity.Object, null);
            var result = routeValidationService.GetFilterForCompanyUser(RouteStatusEnum.Closed);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.AreEqual(result.Result.CompanyID, person.CompanyID);
            Assert.AreEqual(result.Result.AssignedToID, person.ID);
            Assert.AreEqual(result.Result.Status, RouteStatusEnum.Closed);
            Assert.AreEqual(result.Result.NotStatus, RouteStatusEnum.Disabled);
        }

        [TestMethod]
        public void GetFilterForOpenedRoutesFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.USER
            });

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, identity.Object, null);
            var result = routeValidationService.GetFilterForCompanyUser(RouteStatusEnum.Opened);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void GetFilterForCreatedByRoutesSuccefullTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, identity.Object, null);
            var result = routeValidationService.GetFilterForCompanyRouteAndAdmin(RouteStatusEnum.Opened);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.AreEqual(result.Result.CompanyID, person.CompanyID);
            Assert.AreEqual(result.Result.Status, RouteStatusEnum.Opened);
            Assert.AreEqual(result.Result.NotStatus, RouteStatusEnum.Disabled);
        }

        [TestMethod]
        public void GetFilterForCreatedByRoutesFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>());

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, identity.Object, null);
            var result = routeValidationService.GetFilterForCompanyRouteAndAdmin(RouteStatusEnum.Closed);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void GetFilterForGetDetailedSuccefullTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>() {
                RolesName.COMPANY_ROUTE
            });

            var filter = new RouteFilterContract() {
                ID = Guid.NewGuid()
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, identity.Object, null);
            var result = routeValidationService.GetFilterForGetDetailed(filter);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.AreEqual(result.Result.ID, filter.ID);
            Assert.AreEqual(result.Result.CompanyID, person.CompanyID);            
            Assert.AreEqual(result.Result.NotStatus, RouteStatusEnum.Disabled);
        }

        [TestMethod]
        public void GetFilterForGetDetailedRoutesFailTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new List<string>());

            var filter = new RouteFilterContract() {
                ID = Guid.NewGuid()
            };

            var routeValidationService = (IRouteValidationService)new RouteValidationService(null, null, identity.Object, null);
            var result = routeValidationService.GetFilterForGetDetailed(filter);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.AreEqual(result.Result, filter);
        }

        private List<PointDetailedContract> GetPointDetailedContracts()
        {
            return new List<PointDetailedContract>() {
                new PointDetailedContract() {
                    ID = Guid.NewGuid(),
                    Status = PointStatusEnum.Full,
                    PointRouteStatus = PointRouteStatusEnum.Free
                },
                new PointDetailedContract() {
                    ID = Guid.NewGuid(),
                    Status = PointStatusEnum.Full,
                    PointRouteStatus = PointRouteStatusEnum.Free
                }
            };
        }

        private Mock<IPointService> GetPointService()
        {
            return new Mock<IPointService>();
        }

        private Mock<IUserService> GetUserService()
        {
            return new Mock<IUserService>();
        }

        private Mock<IPersonService> GetPersonService()
        {
            return new Mock<IPersonService>();
        }
    }
}