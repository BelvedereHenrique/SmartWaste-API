using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Point;
using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IRouteValidationService
    {
        OperationResult<RouteDetailedContract> CanDisable(RouteDetailedContract route);
        RouteDetailedContract Disable(RouteDetailedContract route);
        OperationResult<RouteDetailedContract> CanRecreate(RouteDetailedContract oldRoute, Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        OperationResult<RouteDetailedContract> CanCreate(Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        OperationResult IsUserAuthorized();
        OperationResult IsAssignedUserValid(Guid? assignedToID);
        OperationResult IsUserAuthorizedToGetRoutes();
        OperationResult<RouteFilterContract> GetFilterForGetDetailed(RouteFilterContract filter);
        OperationResult IsUsersCompanyTheSameOfTheRoute(RouteDetailedContract route);        
        OperationResult AreAllPointsFree(List<PointDetailedContract> points);
        OperationResult AreAllPointsFull(List<PointDetailedContract> points);
        OperationResult IsRouteStatusAbleToDisable(RouteDetailedContract route);
        OperationResult<RouteFilterContract> GetFilterForCompanyUser(RouteStatusEnum? status);
        OperationResult<RouteFilterContract> GetFilterForCompanyRouteAndAdmin(RouteStatusEnum? status);
        OperationResult CanNavigate(RouteDetailedContract route);
        OperationResult CanCollectPoint(RoutePointContract routePoint);
    }
}
