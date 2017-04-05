using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Business.ContractParser;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;

namespace SmartWaste_API.Business
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IRouteHistoryRepository _routeHistoryRepository;
        private readonly IPointRepository _pointRepository;
        private readonly ISecurityManager<IdentityContract> _user;

        public RouteRepository(IRouteHistoryRepository routeHistoryRepository,
                               IPointRepository pointRepository,
                               ISecurityManager<IdentityContract> user)
        {
            _routeHistoryRepository = routeHistoryRepository;
            _pointRepository = pointRepository;
            _user = user;
        }

        public RouteDetailedContract GetDetailed(RouteFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var route = GetFilterDetailedQuery(context, filter).FirstOrDefault().ToContract();

                if (route != null)
                    route.RoutePoints = context.vw_RoutePointsDetailed.Where(x => x.RouteID == route.ID).ToList().ToContracts();

                return route;
            }
        }

        public List<RouteDetailedContract> GetDetailedList(RouteFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var routes = GetFilterDetailedQuery(context, filter).ToList().ToContracts();

                var routeIDs = routes.Select(x => x.ID).ToList();
                var points = context.vw_RoutePointsDetailed.Where(x => routeIDs.Contains(x.RouteID)).ToList();

                routes.ForEach(r => r.RoutePoints = points.Where(p => p.RouteID == r.ID).ToList().ToContracts());

                return routes;
            }
        }

        private IQueryable<Data.Route> GetFilterDetailedQuery(Data.SmartWasteDatabaseConnection context, RouteFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status;

            return context.Routes.Where(x =>
                x.StatusID != (int)filter.NotStatus &&
                x.CompanyID == filter.CompanyID &&
                (filter.ID == null || filter.ID == x.ID) &&
                (statusID == null || statusID == x.StatusID) &&
                (filter.CreatedBy == null || filter.CreatedBy == x.CreatedBy) &&
                (filter.AssignedToID == null || filter.AssignedToID == x.AssignedTo)
            ).AsQueryable();
        }

        public void Create(RouteDetailedContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Create(context, route);
                        SaveHistories(context, histories);
                        EditPoints(context, points, null);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Edit(RouteDetailedContract route, List<RouteHistoryContract> histories, List<RoutePointContract> routePoints, List<PointDetailedContract> points, List<PointHistoryContract> pointHistories)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                Edit(context, route);
                SaveHistories(context, histories);
                EditRoutePoints(context, routePoints);
                EditPoints(context, points, pointHistories);

                context.SaveChanges();
            }
        }

        private void EditRoutePoints(Data.SmartWasteDatabaseConnection context, List<RoutePointContract> routePoints)
        {
            if (routePoints != null)
            {
                var routePointIDs = routePoints.Select(x => x.ID).ToList();

                var entities = context.RoutePoints.Where(x => routePointIDs.Contains(x.ID)).ToList();

                routePoints.ForEach(routePoint =>
                {
                    var r = entities.First(x => x.ID == routePoint.ID);

                    r.IsCollected = routePoint.IsCollected;
                    r.CollectedBy = routePoint.CollectedBy;
                    r.CollectedOn = routePoint.CollectedOn;
                    r.Reason = routePoint.Reason;
                });
            }
        }

        public void Recreate(RouteDetailedContract oldRoute, RouteDetailedContract newRoute, List<RouteHistoryContract> histories, List<PointDetailedContract> points)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                Create(context, newRoute);
                Edit(context, oldRoute, true);
                SaveHistories(context, histories);
                EditPoints(context, points, null);

                context.SaveChanges();
            }
        }

        private void EditPoints(Data.SmartWasteDatabaseConnection context, List<PointDetailedContract> points, List<PointHistoryContract> pointHistories)
        {
            ((IPointInternalRepository)_pointRepository).Edit(context, points, pointHistories);
        }

        private void Edit(Data.SmartWasteDatabaseConnection context, RouteDetailedContract route, bool editRoutePoint = false)
        {
            Data.Route entitie;

            if(editRoutePoint == true)
                entitie = context.Routes.Include("RoutePoints").FirstOrDefault(x => x.ID == route.ID);
            else
                entitie = context.Routes.FirstOrDefault(x => x.ID == route.ID);

            if (entitie == null)
                throw new NullReferenceException("Route does not exist.");

            if (route.AssignedTo == null)
                route.AssignedTo = null;
            else
                entitie.AssignedTo = route.AssignedTo.ID;

            entitie.ClosedOn = route.ClosedOn;
            entitie.StatusID = (int)route.Status;
            entitie.NavigationFinishedOn = route.NavigationFinishedOn;
            entitie.NavigationStartedOn = route.NavigationStartedOn;

            if (editRoutePoint == true)
                foreach (var routePoint in entitie.RoutePoints.ToList())
                {
                    var routePointToEdit = route.RoutePoints.Where(x => x.ID == routePoint.ID).FirstOrDefault();

                    if (routePointToEdit == null)
                        continue;

                    routePoint.CollectedBy = routePointToEdit.CollectedBy;
                    routePoint.CollectedOn = routePointToEdit.CollectedOn;
                    routePoint.IsCollected = routePointToEdit.IsCollected;
                    routePoint.Reason = routePointToEdit.Reason;
                }
        }

        private void SaveHistories(Data.SmartWasteDatabaseConnection context, List<RouteHistoryContract> histories)
        {
            histories.ForEach(history =>
                            ((IRouteHistoryInternalRepository)_routeHistoryRepository).Add(context, history));
        }

        private void Create(Data.SmartWasteDatabaseConnection context, RouteDetailedContract route)
        {
            route.RoutePoints.ForEach((routePoint) =>
            {
                context.RoutePoints.Add(new Data.RoutePoint()
                {
                    ID = routePoint.ID,
                    PointID = routePoint.Point.ID,
                    RouteID = route.ID,
                    CollectedBy = routePoint.CollectedBy,
                    CollectedOn = routePoint.CollectedOn,
                    Reason = routePoint.Reason,
                    IsCollected = routePoint.IsCollected
                });
            });

            context.Routes.Add(route.ToEntitie());
        }

        public List<RouteContract> GetCompanyUserRoutes(RouteFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                int? statusID = null;

                if (filter.Status.HasValue)
                    statusID = (int)filter.Status;

                return context.vw_GetRoutes.Where(x =>
                   x.CompanyID == filter.CompanyID &&
                   (
                       x.AssignedToID == filter.AssignedToID ||
                       x.AssignedToID == null
                   ) &&
                   (statusID == null || x.StatusID == statusID) &&
                   x.StatusID != (int)filter.NotStatus
                ).OrderByDescending(x => x.CreatedOn).ToList().ToContracts();
            }
        }

        public List<RouteContract> GetCompanyAdminRoutes(RouteFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status;

            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return context.vw_GetRoutes.Where(x =>
                   x.CompanyID == filter.CompanyID &&
                   (filter.CreatedBy == null || x.CreatedByID == filter.CreatedBy) &&
                   x.StatusID != (int)filter.NotStatus &&
                   (statusID == null || x.StatusID == statusID)
                ).OrderBy(x => x.CreatedOn).ToList().ToContracts();
            }
        }
    }
}
