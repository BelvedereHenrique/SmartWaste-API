using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IRouteService
    {
        OperationResult<Guid> Recreate(Guid oldRouteID, Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        OperationResult<Guid> Create(Guid? assignedToID, List<Guid> pointIDs, Decimal expectedKilometers, Decimal expectedMinutes);
        RouteDetailedContract GetDetailed(RouteFilterContract filter);
        List<RouteDetailedContract> GetDetailedList(RouteFilterContract filter);
        OperationResult Disable(Guid routeID);
        List<RouteContract> GetList(RouteStatusEnum? status);        
        OperationResult<RouteDetailedContract> StartNavigation(Guid routeID);
        OperationResult CollectPoint(Guid routeID, Guid pointID, bool collected, string reason);
    }
}
