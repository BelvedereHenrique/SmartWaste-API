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

            _routeRepository.Create(newRouteResult.Result, newRouteResult.Result.Histories, newRouteResult.Result.Points);

            result.Result = newRouteResult.Result.ID;

            return result;
        }

        public RouteDetailedContract GetDetailed(RouteFilterContract filter)
        {
            var filterResult = _routeValidationService.GetFilterForGetDetailed(filter);

            if (!filterResult.Success)
                throw new Exception(filterResult.GetMessage(true));

            return _routeRepository.GetDetailed(filterResult.Result);
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

            _routeRepository.Edit(result.Result, histories, result.Result.Points);

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

            var oldPointsToUpdate = oldRoute.Points.Where((point) => {
                return canRecreateResult.Result.Points.All((newPoint) => newPoint.ID != point.ID);
            }).ToList();

            oldPointsToUpdate.AddRange(canRecreateResult.Result.Points);

            _routeRepository.Recreate(oldRoute, canRecreateResult.Result, histories, oldPointsToUpdate);

            result.Result = canRecreateResult.Result.ID;

            return result;
        }
        
        private List<RouteHistoryContract> GetDisableHistories(RouteDetailedContract route)
        {
            var histories = new List<RouteHistoryContract>();
            histories.Add(new RouteHistoryContract()
            {
                ID = Guid.NewGuid(),
                Person = new SmarteWaste_API.Contracts.Person.PersonContract() {
                    ID = _user.User.Person.ID
                },
                Reason = "Route removed.",
                RouteID = route.ID,
                Status = RouteStatusEnum.Disabled,
                Date = DateTime.Now
            });

            return histories;
        }
        
        public List<RouteContract> GetOpenedRoutes()
        {
            var result = _routeValidationService.GetFilterForOpenedRoutes();

            if (!result.Success)
                throw new Exception(result.GetMessage(true));

            return _routeRepository.GetOpenedRoutes(result.Result);
        }

        public List<RouteContract> GetUserCreatedRoutes()
        {
            var result = _routeValidationService.GetFilterForCreatedByRoutes();

            if (!result.Success)
                throw new Exception(result.GetMessage(true));

            return _routeRepository.GetUserCreatedRoutes(result.Result);
        }
    }
}
