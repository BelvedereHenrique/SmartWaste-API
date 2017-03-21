using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.Point;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Services.Interfaces;
using SmartWaste_API.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class RouteValidationService : IRouteValidationService
    {
        private readonly IUserService _userService;
        private readonly IPersonService _personService;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IPointService _pointService;

        private const string USER_NOT_AUTHORIZED = "User is not authorized.";
        private const string INVALID_ASSIGNED_TO = "{0} cannot be assigned to the route.";
        private const string NO_POINTS_IN_ROUTE_MESSAGE = "The route must have points.";
        private const string POINTS_NOT_FULL_MESSAGE = "All points must be full.";
        private const string POINTS_NOT_FREE_MESSAGE = "All points must be not in other routes.";
        private const string ROUTE_STATUS_ERROR_TO_DISABLE = "Route is not opened to be disabled.";
        private const string ROUTE_DOESNT_BELONG_TO_USERS_COMPANY = "Route doesn't belong to your company to remove it.";
        private const string ROUTE_CREATED_STATUS_REASON = "Route created.";

        public RouteValidationService(IUserService userService,
                                      IPersonService personService,
                                      ISecurityManager<IdentityContract> user,
                                      IPointService pointService)
        {
            _userService = userService;
            _personService = personService;
            _user = user;
            _pointService = pointService;
        }

        public OperationResult<RouteDetailedContract> CanCreate(Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes)
        {
            var result = new OperationResult<RouteDetailedContract>();

            result.Merge(IsUserAuthorized());
            result.Merge(IsAssignedUserValid(assignedToID));

            var points = _pointService.GetDetailedList(new PointFilterContract()
            {
                IDs = pointIDs
            });

            result.Merge(AreAllPointsFull(points));
            result.Merge(AreAllPointsFree(points));

            if (!result.Success)
                return result;

            result.Result = BuildNewRouteContract(assignedToID, points, expectedKilometers, expectedMinutes);

            return result;
        }

        public OperationResult<RouteDetailedContract> CanRecreate(RouteDetailedContract oldRoute, Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes)
        {
            var result = new OperationResult<RouteDetailedContract>();

            result.Merge(IsUserAuthorized());
            result.Merge(IsAssignedUserValid(assignedToID));

            var points = _pointService.GetDetailedList(new PointFilterContract()
            {
                IDs = pointIDs
            });

            points.Where(point => oldRoute.Points.Any(oldPoint => oldPoint.ID == point.ID)).ToList().ForEach((point) =>
            {
                point.PointRouteStatus = PointRouteStatusEnum.Free;
            });

            result.Merge(AreAllPointsFull(points));
            result.Merge(AreAllPointsFree(points));

            if (!result.Success)
                return result;

            result.Result = BuildNewRouteContract(assignedToID, points, expectedKilometers, expectedMinutes);

            result.Merge(CanDisable(oldRoute));

            return result;

        }

        public OperationResult<RouteDetailedContract> CanDisable(RouteDetailedContract route)
        {
            var result = new OperationResult<RouteDetailedContract>();

            result.Merge(IsUserAuthorized());
            result.Merge(IsUsersCompanyTheSameOfTheRoute(route));
            result.Merge(IsRouteStatusAbleToDisable(route));

            result.Result = Disable(route);

            return result;
        }

        public RouteDetailedContract Disable(RouteDetailedContract route)
        {
            route.Status = RouteStatusEnum.Disabled;

            route.Points.ForEach((point) =>
            {
                point.PointRouteStatus = PointRouteStatusEnum.Free;
            });

            return route;
        }

        public OperationResult IsRouteStatusAbleToDisable(RouteDetailedContract route)
        {
            var result = new OperationResult();

            if (route.Status != RouteStatusEnum.Opened)
                result.AddError(ROUTE_STATUS_ERROR_TO_DISABLE);

            return result;
        }

        public OperationResult IsUsersCompanyTheSameOfTheRoute(RouteDetailedContract route)
        {
            var result = new OperationResult();

            if (route.CompanyID != _user.User.Person.CompanyID)
                result.AddError(ROUTE_DOESNT_BELONG_TO_USERS_COMPANY);

            return result;
        }

        public OperationResult IsUserAuthorized()
        {
            var result = new OperationResult();

            if (!_user.User.IsAuthenticated || !_user.IsInRole(RolesName.COMPANY_ROUTE))
                result.AddError(USER_NOT_AUTHORIZED);

            return result;
        }

        public OperationResult IsAssignedUserValid(Guid? assignedToID)
        {
            var result = new OperationResult();

            if (assignedToID.HasValue)
            {
                var assignedTo = this._personService.Get(new PersonFilterContract()
                {
                    ID = assignedToID.Value
                });

                var assignedToUser = this._userService.Get(new SmarteWaste_API.Contracts.User.UserFilterContract()
                {
                    ID = assignedTo.UserID
                });

                if (assignedTo.CompanyID == null || assignedTo.CompanyID != _user.User.Person.CompanyID)
                    result.AddError(String.Format(INVALID_ASSIGNED_TO, assignedTo.Name));
            }

            return result;
        }

        public OperationResult AreAllPointsFree(List<PointDetailedContract> points)
        {
            var result = new OperationResult();

            if (points.Any(x => x.PointRouteStatus != PointRouteStatusEnum.Free))
                result.AddError(POINTS_NOT_FREE_MESSAGE);

            return result;
        }

        public OperationResult AreAllPointsFull(List<PointDetailedContract> points)
        {
            var result = new OperationResult();

            if (points.Any(x => x.Status != PointStatusEnum.Full))
                result.AddError(POINTS_NOT_FULL_MESSAGE);

            return result;
        }

        public OperationResult IsUserAuthorizedToGetRoutes()
        {
            var result = new OperationResult<RouteFilterContract>();

            if (!_user.User.IsAuthenticated ||
                !_user.User.Person.CompanyID.HasValue ||
                (
                    !_user.IsInRole(RolesName.COMPANY_ADMIN) &&
                    !_user.IsInRole(RolesName.COMPANY_ROUTE) &&
                    !_user.IsInRole(RolesName.COMPANY_USER)
                )
               )
                result.AddError(USER_NOT_AUTHORIZED);

            return result;
        }
        
        public OperationResult<RouteFilterContract> GetFilterForGetDetailed(RouteFilterContract filter)
        {
            var result = new OperationResult<RouteFilterContract>(filter);

            result.Merge(IsUserAuthorizedToGetRoutes());

            if (!result.Success)
                return result;
            
            result.Result.CompanyID = _user.User.Person.CompanyID;
            result.Result.NotStatus = RouteStatusEnum.Disabled;
            
            return result;
        }

        public OperationResult<RouteFilterContract> GetFilterForOpenedRoutes()
        {
            var result = new OperationResult<RouteFilterContract>();

            result.Merge(IsUserAuthorizedToGetRoutes());

            if (!result.Success)
                return result;

            result.Result = new RouteFilterContract();

            result.Result.Status = RouteStatusEnum.Opened;
            result.Result.AssignedToID = _user.User.Person.ID;
            result.Result.CompanyID = _user.User.Person.CompanyID;

            return result;
        }

        public OperationResult<RouteFilterContract> GetFilterForCreatedByRoutes()
        {
            var result = new OperationResult<RouteFilterContract>();

            result.Merge(IsUserAuthorizedToGetRoutes());

            if (!result.Success)
                return result;

            result.Result = new RouteFilterContract();

            result.Result.NotStatus = RouteStatusEnum.Disabled;
            result.Result.CreatedBy = _user.User.Person.ID;
            result.Result.CompanyID = _user.User.Person.CompanyID;

            return result;
        }

        private RouteDetailedContract BuildNewRouteContract(Guid? assignedToID, List<PointDetailedContract> points, Decimal expectedKilometers, Decimal expectedMinutes)
        {
            var route = new RouteDetailedContract()
            {
                ID = Guid.NewGuid(),
                CreatedBy = new PersonContract()
                {
                    ID = _user.User.Person.ID
                },
                CreatedOn = DateTime.Now,
                Status = RouteStatusEnum.Opened,
                Points = points,
                ClosedOn = null,
                AssignedTo = null,
                CompanyID = _user.User.Person.CompanyID.Value,
                ExpectedKilometers = expectedKilometers,
                ExpectedMinutes = expectedMinutes
            };

            route.Points.ForEach((point) =>
            {
                point.PointRouteStatus = PointRouteStatusEnum.InARoute;
            });

            if (assignedToID.HasValue)
                route.AssignedTo = new PersonContract()
                {
                    ID = assignedToID.Value
                };

            route.Histories.Add(new RouteHistoryContract()
            {
                ID = Guid.NewGuid(),
                Person = new PersonContract()
                {
                    ID = _user.User.Person.ID
                },
                Reason = ROUTE_CREATED_STATUS_REASON,
                RouteID = route.ID,
                Status = RouteStatusEnum.Opened,
                Date = DateTime.Now
            });

            return route;
        }
    }
}