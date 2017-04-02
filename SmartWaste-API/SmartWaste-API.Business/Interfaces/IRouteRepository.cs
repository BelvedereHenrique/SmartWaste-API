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
        void Recreate(RouteDetailedContract oldRoute, RouteDetailedContract newRoute, List<RouteHistoryContract> histories, List<PointDetailedContract> points);
        void Create(RouteDetailedContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points);
        RouteDetailedContract GetDetailed(RouteFilterContract filter);
        List<RouteDetailedContract> GetDetailedList(RouteFilterContract filter);        
        void Edit(RouteDetailedContract route, List<RouteHistoryContract> histories, List<RoutePointContract> routePoints, List<PointDetailedContract> points, List<PointHistoryContract> pointHistories);
        List<RouteContract> GetCompanyUserRoutes(RouteFilterContract filter);
        List<RouteContract> GetCompanyAdminRoutes(RouteFilterContract filter);
    }
}
