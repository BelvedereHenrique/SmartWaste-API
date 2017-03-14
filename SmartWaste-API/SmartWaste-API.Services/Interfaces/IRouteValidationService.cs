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
        OperationResult<RouteContract> CanDisable(RouteContract route);
        RouteContract Disable(RouteContract route);
        OperationResult<RouteContract> CanRecreate(RouteContract oldRoute, Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        OperationResult<RouteContract> CanCreate(Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        OperationResult IsUserAuthorized();
        OperationResult IsAssignedUserValid(Guid? assignedToID);
        OperationResult IsUserAuthorizedToGetRoutes();
        OperationResult<RouteFilterContract> CheckFilter(RouteFilterContract filter);
        OperationResult IsUsersCompanyTheSameOfTheRoute(RouteContract route);        
        OperationResult AreAllPointsFree(List<PointDetailedContract> points);
        OperationResult AreAllPointsFull(List<PointDetailedContract> points);
        OperationResult IsRouteStatusAbleToDisable(RouteContract route);
    }
}
