using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Business.ContractParser;
using SmarteWaste_API.Contracts.Point;

namespace SmartWaste_API.Business
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IRouteHistoryRepository _routeHistoryRepository;
        private readonly IPointRepository _pointRepository;

        public RouteRepository(IRouteHistoryRepository routeHistoryRepository,
                               IPointRepository pointRepository)
        {
            _routeHistoryRepository = routeHistoryRepository;
            _pointRepository = pointRepository;
        }

        public RouteContract Get(RouteFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var route = GetFilterQuery(context, filter).FirstOrDefault().ToContract();
                var pointIDs = route.Points.Select(x => x.ID).ToList();
                route.Points = context.vw_GetPointsDetailed.Where(x => pointIDs.Contains(x.ID)).ToList().ToContracts();

                return route;
            }
        }

        public List<RouteContract> GetList(RouteFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var routes = GetFilterQuery(context, filter).ToList().ToContracts();

                var pointIDs = new List<Guid>();
                routes.ForEach(r => pointIDs.AddRange(r.Points.Select(x => x.ID).ToList()));

                var points = context.vw_GetPointsDetailed.Where(x => pointIDs.Contains(x.ID)).ToList().ToContracts();

                routes.ForEach(r => r.Points = points.Where(p => r.Points.Any(s => s.ID == p.ID)).ToList());

                return routes;
            }
        }

        private IQueryable<Data.Route> GetFilterQuery(Data.SmartWasteDatabaseConnection context, RouteFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status;

            return context.Routes.Where(x =>
                x.StatusID != (int)RouteStatusEnum.Disabled &&
                (filter.ID == null || filter.ID == x.ID) &&
                (statusID == null || statusID == x.StatusID) &&
                (filter.CreatedBy == null || filter.CreatedBy == x.CreatedBy) &&                
                (   
                    filter.LoadUnassigned == null ||
                        (
                            filter.AssignedToID == null || filter.AssignedToID == x.AssignedTo
                        ) ||
                        (
                            filter.LoadUnassigned == true && x.AssignedTo == null
                        ) ||
                        (
                            filter.LoadUnassigned == false && x.AssignedTo != null
                        )                 
                )
            ).AsQueryable();
        }

        public void Create(RouteContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Create(context, route);
                        SaveHistories(context, histories);
                        EditPoints(context, points);

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
        
        public void Edit(RouteContract route, List<RouteHistoryContract> histories, List<PointDetailedContract> points)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                Edit(context, route);
                SaveHistories(context, histories);
                EditPoints(context, points);

                context.SaveChanges();
            }
        }

        public void Recreate(RouteContract oldRoute, RouteContract newRoute, List<RouteHistoryContract> histories, List<PointDetailedContract> points)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                Create(context, newRoute);
                Edit(context, oldRoute);
                SaveHistories(context, histories);
                EditPoints(context, points);

                context.SaveChanges();
            }
        }

        private void EditPoints(Data.SmartWasteDatabaseConnection context,  List<PointDetailedContract> points)
        {
            points.ForEach((point) => {
                ((IPointInternalRepository)_pointRepository).Edit(context, point);
            });
        }

        private void Edit(Data.SmartWasteDatabaseConnection context, RouteContract route)
        {
            var entitie = context.Routes.Find(route.ID);

            if (entitie == null)
                throw new NullReferenceException("Route does not exist.");

            if (route.AssignedTo == null)
                route.AssignedTo = null;
            else
                entitie.AssignedTo = route.AssignedTo.ID;

            entitie.ClosedOn = route.ClosedOn;
            entitie.StatusID = (int)route.Status;            
        }

        private void SaveHistories(Data.SmartWasteDatabaseConnection context, List<RouteHistoryContract> histories)
        {
            histories.ForEach(history =>
                            ((IRouteHistoryInternalRepository)_routeHistoryRepository).Add(context, history));
        }

        private void Create(Data.SmartWasteDatabaseConnection context, RouteContract route)
        {
            route.Points.ForEach((point) =>
            {
                context.RoutePoints.Add(new Data.RoutePoint()
                {
                    ID = Guid.NewGuid(),
                    PointID = point.ID,
                    RouteID = route.ID
                });
            });

            context.Routes.Add(route.ToEntitie());
        }
    }
}
