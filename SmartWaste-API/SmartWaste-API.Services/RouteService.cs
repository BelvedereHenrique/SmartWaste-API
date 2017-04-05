using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.OperationResult;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Security;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Business.Interfaces;
using SmarteWaste_API.Contracts.Point;

namespace SmartWaste_API.Services
{
    public class RouteService : IRouteService
    {
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IRouteRepository _routeRepository;
        private readonly IRouteValidationService _routeValidationService;

        private List<string> AUTHORIZED_ROLES_TO_THE_ASSIGNED_USER = new List<string>() {
            RolesName.COMPANY_ADMIN,
            RolesName.COMPANY_ROUTE,
            RolesName.COMPANY_USER
        };

        public RouteService(ISecurityManager<IdentityContract> user,
                            IRouteRepository routeRepository,
                            IRouteValidationService routeValidationService)
        {
            this._user = user;
            this._routeRepository = routeRepository;
            this._routeValidationService = routeValidationService;
        }

        public OperationResult<Guid> Create(Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes)
        {
            var result = new OperationResult<Guid>();

            OperationResult<RouteDetailedContract> newRouteResult = _routeValidationService.CanCreate(assignedToID, pointIDs, expectedKilometers, expectedMinutes);

            result.Merge(newRouteResult);

            if (!result.Success)
                return result;

            _routeRepository.Create(newRouteResult.Result, newRouteResult.Result.Histories, newRouteResult.Result.RoutePoints.Select(x => x.Point).ToList());

            result.Result = newRouteResult.Result.ID;

            return result;
        }

        public RouteDetailedContract GetDetailed(RouteFilterContract filter)
        {
            var filterResult = _routeValidationService.GetFilterForGetDetailed(filter);

            if (!filterResult.Success)
                throw new Exception(filterResult.GetMessage(true));

            var route = _routeRepository.GetDetailed(filterResult.Result);

            if (route != null && route.Histories != null)
                route.Histories = route.Histories.OrderByDescending(x => x.Date).ToList();

            return route;
        }

        public List<RouteDetailedContract> GetDetailedList(RouteFilterContract filter)
        {
            var filterResult = _routeValidationService.GetFilterForGetDetailed(filter);

            if (!filterResult.Success)
                throw new Exception(filterResult.GetMessage(true));

            return _routeRepository.GetDetailedList(filterResult.Result);
        }

        public OperationResult Disable(Guid routeID)
        {
            var route = GetDetailed(new RouteFilterContract()
            {
                ID = routeID
            });

            var result = _routeValidationService.CanDisable(route);

            if (!result.Success)
                return result;

            var histories = GetDisableHistories(result.Result);

            _routeRepository.Edit(result.Result, histories, result.Result.RoutePoints, result.Result.RoutePoints.Select(x => x.Point).ToList(), null);

            return result;
        }

        public OperationResult<Guid> Recreate(Guid oldRouteID, Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes)
        {
            var result = new OperationResult<Guid>();

            var route = GetDetailed(new RouteFilterContract()
            {
                ID = oldRouteID
            });

            var canRecreateResult = _routeValidationService.CanRecreate(route, assignedToID, pointIDs, expectedKilometers, expectedMinutes);
            result.Merge(canRecreateResult);

            if (!result.Success)
                return result;

            var oldRoute = _routeValidationService.Disable(route);

            var histories = GetDisableHistories(oldRoute);
            histories.AddRange(canRecreateResult.Result.Histories);

            var oldPointsToUpdate = oldRoute.RoutePoints.Where((point) =>
            {
                return canRecreateResult.Result.RoutePoints.All((newPoint) => newPoint.Point.ID != point.Point.ID);
            }).ToList();

            oldPointsToUpdate.AddRange(canRecreateResult.Result.RoutePoints);

            _routeRepository.Recreate(oldRoute, canRecreateResult.Result, histories, oldPointsToUpdate.Select(x => x.Point).ToList());

            result.Result = canRecreateResult.Result.ID;

            return result;
        }

        private List<RouteHistoryContract> GetDisableHistories(RouteDetailedContract route)
        {
            var histories = new List<RouteHistoryContract>();
            histories.Add(new RouteHistoryContract()
            {
                ID = Guid.NewGuid(),
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = _user.User.Person.ID
                },
                Reason = "Route removed.",
                RouteID = route.ID,
                Status = RouteStatusEnum.Disabled,
                Date = DateTime.Now
            });

            return histories;
        }

        public List<RouteContract> GetList(RouteStatusEnum? status)
        {
            var result = new OperationResult<RouteFilterContract>();

            var isAdminUser = _user.IsInRole(RolesName.COMPANY_ADMIN) || _user.IsInRole(RolesName.COMPANY_ROUTE);

            if (isAdminUser)
                result = _routeValidationService.GetFilterForCompanyRouteAndAdmin(status);
            else
                result = _routeValidationService.GetFilterForCompanyUser(status);

            if (!result.Success)
                throw new Exception(result.GetMessage(true));

            if (isAdminUser)
                return _routeRepository.GetCompanyAdminRoutes(result.Result);
            else
                return _routeRepository.GetCompanyUserRoutes(result.Result);
        }

        public OperationResult<RouteDetailedContract> StartNavigation(Guid routeID)
        {
            var result = new OperationResult<RouteDetailedContract>();

            var route = GetDetailed(new RouteFilterContract() { ID = routeID });

            result.Merge(_routeValidationService.CanNavigate(route));

            if (!result.Success)
                return result;

            var histories = new List<RouteHistoryContract>();

            histories.Add(new RouteHistoryContract()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                Person = _user.User.Person,
                RouteID = route.ID,
                Reason = "Navigation started.",
                Status = route.Status
            });

            if (!route.NavigationStartedOn.HasValue)
                route.NavigationStartedOn = DateTime.Now;

            if (route.AssignedTo == null || route.AssignedTo.ID == Guid.Empty)
            {
                route.AssignedTo = _user.User.Person;

                histories.Add(new RouteHistoryContract()
                {
                    ID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Person = _user.User.Person,
                    RouteID = route.ID,
                    Reason = String.Format("{0} assigned to the route.", _user.User.Person.Name),
                    Status = route.Status
                });
            }

            _routeRepository.Edit(route, histories, null, null, null);

            result.Result = GetDetailed(new RouteFilterContract() { ID = routeID });
            return result;
        }

        public OperationResult CollectPoint(Guid routeID, Guid pointID, bool collected, string reason)
        {
            var result = new OperationResult();

            var route = GetDetailed(new RouteFilterContract() { ID = routeID });

            result.Merge(_routeValidationService.CanNavigate(route));

            if (!result.Success)
                return result;

            var routePoint = route.RoutePoints.Where(x => x.Point.ID == pointID).FirstOrDefault();

            result.Merge(_routeValidationService.CanCollectPoint(routePoint));

            if (!result.Success)
                return result;

            var pointHistories = new List<PointHistoryContract>();
            routePoint.Point.PointRouteStatus = PointRouteStatusEnum.Free;

            routePoint.IsCollected = collected;
            routePoint.CollectedBy = _user.User.Person.ID;
            routePoint.CollectedOn = DateTime.Now;

            if (collected)
            {
                routePoint.Point.Status = PointStatusEnum.Empty;
                pointHistories.Add(new PointHistoryContract()
                {
                    Date = DateTime.Now,
                    ID = Guid.NewGuid(),
                    Person = _user.User.Person,
                    PointID = routePoint.Point.ID,
                    Reason = "Point collected.",
                    Status = routePoint.Point.Status
                });
            }
            else
            {
                routePoint.Reason = reason;

                pointHistories.Add(new PointHistoryContract()
                {
                    Date = DateTime.Now,
                    ID = Guid.NewGuid(),
                    Person = _user.User.Person,
                    PointID = routePoint.Point.ID,
                    Reason = String.Format("Tried to collect point. Reason: {0}.", reason),
                    Status = routePoint.Point.Status
                });
            }

            var routeHistory = new List<RouteHistoryContract>();

            if (route.RoutePoints.Where(x => x.ID != routePoint.ID).All(x => x.IsCollected != null))
            {
                route.Status = RouteStatusEnum.Closed;
                route.ClosedOn = DateTime.Now;
                route.NavigationFinishedOn = DateTime.Now;

                routeHistory.Add(new RouteHistoryContract()
                {
                    ID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Person = _user.User.Person,
                    RouteID = route.ID,
                    Reason = "Finished navigation and route closed.",
                    Status = route.Status
                });
            }

            _routeRepository.Edit(route,
                                  routeHistory,
                                  route.RoutePoints,
                                  new List<PointDetailedContract>() { routePoint.Point },
                                  pointHistories);

            return result;
        }
    }
}
