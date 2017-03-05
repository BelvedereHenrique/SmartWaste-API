using SmarteWaste_API.Contracts.Point;
using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IRouteRepository
    {
        void Recreate(RouteContract oldRoute, RouteContract newRoute, List<RouteHistoryContract> histories, List<PointDetailedContract> points);
        void Create(RouteContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points);
        RouteContract Get(RouteFilterContract filter);
        List<RouteContract> GetList(RouteFilterContract filter);
        void Edit(RouteContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points);
    }
}
