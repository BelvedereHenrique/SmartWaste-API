using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Business.ContractParser;
using SmartWaste_API.Business.Data;
using SmarteWaste_API.Contracts.Person;

namespace SmartWaste_API.Business
{
    public class PointRepository : IPointRepository, IPointInternalRepository
    {
        private readonly IPointHistoryRepository _pointHistoryRepository;

        public PointRepository(IPointHistoryRepository pointHistoryRepository)
        {
            _pointHistoryRepository = pointHistoryRepository;
        }

        public List<PointDetailedContract> GetDetailedList(PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetPointsDetailedQuery(context, filter).ToList().ToContracts();
            }
        }

        public IQueryable<vw_GetPointsDetailed> GetPointsDetailedQuery(Data.SmartWasteDatabaseConnection context, PointFilterContract filter)
        {
            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_GetPointsDetailed.Where(x =>
                            (
                                (filter.Northwest.Latitude == null || x.Latitude <= filter.Northwest.Latitude) &&
                                (filter.Southeast.Latitude == null || x.Latitude >= filter.Southeast.Latitude)
                            ) &&
                            (
                                (
                                    (filter.AlwaysIDs.Contains(x.ID))
                                )
                                    ||
                                (

                                    (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                                    (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude) &&
                                    (filter.PersonID == null || filter.PersonID == x.PersonID) &&
                                    (statusID == null || statusID == x.StatusID) &&
                                    (typeID == null || typeID == x.TypeID) &&
                                    (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                                    (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                                    (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID)
                                )
                            )
            ).AsQueryable();
        }

        public List<PointContract> GetList(PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetPointsQuery(context, filter).ToList().ToContracts();
            }
        }

        public IQueryable<vw_GetPoints> GetPointsQuery(Data.SmartWasteDatabaseConnection context, PointFilterContract filter)
        {
            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_GetPoints.Where(x =>
                            (
                                (filter.Northwest.Latitude == null || x.Latitude <= filter.Northwest.Latitude) &&
                                (filter.Southeast.Latitude == null || x.Latitude >= filter.Southeast.Latitude)
                            ) &&
                            (
                                (
                                    (filter.AlwaysIDs.Contains(x.ID))
                                )
                                    ||
                                (

                                    (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                                    (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude) &&
                                    (filter.PersonID == null || filter.PersonID == x.PersonID) &&
                                    (statusID == null || statusID == x.StatusID) &&
                                    (typeID == null || typeID == x.TypeID) &&
                                    (filter.DeviceID == null || x.DeviceID == filter.DeviceID) &&
                                    (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                                    (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                                    (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID)
                                )
                            )
                        ).AsQueryable();
        }

        public void Edit(PointContract point, List<PointHistoryContract> histories)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                ((IPointInternalRepository)this).Edit(context, point, histories);

                context.SaveChanges();
            }
        }

        void IPointInternalRepository.Edit(SmartWasteDatabaseConnection context, PointContract point, List<PointHistoryContract> histories)
        {
            var entitie = context.Points.Find(point.ID);

            entitie.TypeID = (int)point.Type;
            entitie.StatusID = (int)point.Status;
            entitie.AddressID = point.AddressID;
            entitie.DeviceID = point.DeviceID;
            entitie.PointRouteStatusID = (int)point.PointRouteStatus;

            if (histories != null)
                histories.ForEach((history) =>
                    ((IPointHistoryInternalRepository)_pointHistoryRepository).Add(context, history)
                );
        }

        public PointDetailedContract GetDetailed(PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return context.vw_GetPointsDetailed.Where(x =>
                    (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                    (filter.PersonID == null || filter.PersonID == x.PersonID)
                ).FirstOrDefault().ToContract();
            }
        }

        public PointContract GetPointByDeviceID(Guid deviceID)
        {
            using (var context = new SmartWasteDatabaseConnection())
            {
                return context.vw_GetPoints.FirstOrDefault(x => x.DeviceID == deviceID).ToContract();
            }
        }

        public void AddCompanyPoint(PointContract contract, Guid? companyID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var point = new Point()
                        {
                            ID = Guid.NewGuid(),
                            AddressID = contract.AddressID,
                            StatusID = (int)contract.Status,
                            DeviceID = contract.DeviceID,
                            TypeID = (int)contract.Type,
                            PointRouteStatusID = (int)contract.PointRouteStatus
                        };


                        var companyAddress = new CompanyAddress()
                        {
                            AddressID = contract.AddressID,
                            CompanyID = companyID.Value,
                            ID = Guid.NewGuid()
                        };

                        context.CompanyAddresses.Add(companyAddress);

                        context.Points.Add(point);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
