﻿using SmarteWaste_API.Contracts.OperationResult;
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
        RouteContract Get(RouteFilterContract filter);
        List<RouteContract> GetList(RouteFilterContract filter);
        OperationResult Disable(Guid routeID);
    }
}
