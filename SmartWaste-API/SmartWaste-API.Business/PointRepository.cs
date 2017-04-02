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
        
        public List<PointDetailedContract> GetUserDetailedList(Guid personID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetUserDetailedQuery(context, personID, filter).ToList().ToContracts();
            }
        }

        public PointDetailedContract GetUserDetailed(Guid personID, PointFilterContract filter)
        {
            using (var context = new SmartWasteDatabaseConnection())
            {
                return GetUserDetailedQuery(context, personID, filter).FirstOrDefault().ToContract();
            }
        }

        private IQueryable<Data.vw_PointsDetailed2> GetUserDetailedQuery(Data.SmartWasteDatabaseConnection context, Guid personID, PointFilterContract filter)
        {
            if (personID == null || personID == Guid.Empty)
                throw new ArgumentNullException("PersonID");

            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_PointsDetailed2.Where(x =>
                // NOTE: Security and required filter.
                (x.PersonID == filter.PersonID || x.TypeID == (int)PointTypeEnum.CompanyTrashCan) &&

                (statusID == null || statusID == x.StatusID) &&
                (typeID == null || typeID == x.TypeID) &&
                (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID) &&
                (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude)
            );            
        }

        public PointContract GetUserPoint(Guid personID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetUserQuery(context, personID, filter).FirstOrDefault().ToContract();
            }
        }

        public List<PointContract> GetUserList(Guid personID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetUserQuery(context, personID, filter).ToList().ToContracts();
            }
        }

        private IQueryable<Data.vw_Points2> GetUserQuery(Data.SmartWasteDatabaseConnection context, Guid personID, PointFilterContract filter)
        {
            if (personID == null || personID == Guid.Empty)
                throw new ArgumentNullException("PersonID");

            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_Points2.Where(x =>
                // NOTE: Security and required filter.
                (x.PersonID == filter.PersonID || x.TypeID == (int)PointTypeEnum.CompanyTrashCan) &&

                (statusID == null || statusID == x.StatusID) &&
                (typeID == null || typeID == x.TypeID) &&
                (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID) &&
                (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude)
            );
        }

        private IQueryable<Data.vw_Points2> GetCompanyQuery(Data.SmartWasteDatabaseConnection context, Guid companyID, PointFilterContract filter)
        {
            if (companyID == null || companyID == Guid.Empty)
                throw new ArgumentNullException("CompanyID");

            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_Points2.Where(x =>
                // NOTE: Security and required filters.
                (x.CompanyID == null || x.CompanyID == companyID) &&
                (x.AssignedCompanyID == null || x.AssignedCompanyID == companyID) &&


                (statusID == null || statusID == x.StatusID) &&
                (typeID == null || typeID == x.TypeID) &&
                (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID) &&
                (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude)
            );
        }

        public PointContract GetCompanyPoint(Guid companyID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetCompanyQuery(context, companyID, filter).FirstOrDefault().ToContract();
            }
        }

        public List<PointContract> GetCompanyList(Guid companyID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetCompanyQuery(context, companyID, filter).ToList().ToContracts();
            }
        }

        private IQueryable<Data.vw_PointsDetailed2> GetCompanyDetailedQuery(Data.SmartWasteDatabaseConnection context, Guid companyID, PointFilterContract filter)
        {
            if (companyID == null || companyID == Guid.Empty)
                throw new ArgumentNullException("CompanyID");

            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_PointsDetailed2.Where(x =>
                // NOTE: Security and required filters.
                (x.CompanyID == null || x.CompanyID == companyID) &&
                (x.AssignedCompanyID == null || x.AssignedCompanyID == companyID) &&


                (statusID == null || statusID == x.StatusID) &&
                (typeID == null || typeID == x.TypeID) &&
                (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID) &&
                (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude)
            );
        }

        public PointDetailedContract GetCompanyDetailed(Guid companyID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetCompanyDetailedQuery(context, companyID, filter).FirstOrDefault().ToContract();
            }
        }

        public List<PointDetailedContract> GetCompanyDetailedList(Guid companyID, PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetCompanyDetailedQuery(context, companyID, filter).ToList().ToContracts();
            }
        }

        private IQueryable<Data.vw_Points2> GetPublicDetailedQuery(Data.SmartWasteDatabaseConnection context, PointFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            return context.vw_Points2.Where(x =>
                // NOTE: Security and required filters.
                x.TypeID == (int)PointTypeEnum.CompanyTrashCan &&

                (statusID == null || statusID == x.StatusID) &&
                (filter.IDs.Count == 0 || filter.IDs.Contains(x.ID)) &&
                (filter.NotIDs.Count == 0 || !filter.NotIDs.Contains(x.ID)) &&
                (filter.Northwest.Longitude == null || x.Latitude >= filter.Northwest.Longitude) &&
                (filter.Southeast.Longitude == null || x.Longitude <= filter.Southeast.Longitude)
            );
        }

        public PointContract GetPublicPoint(PointFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetPublicDetailedQuery(context, filter).FirstOrDefault().ToContract();
            }
        }

        public List<PointContract> GetPublicList(PointFilterContract filter)
        {
            int? statusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;            

            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetPublicDetailedQuery(context, filter).ToList().ToContracts();
            }
        }
        
        public IQueryable<Data.vw_Points2> GetPointsQuery(Data.SmartWasteDatabaseConnection context, PointFilterContract filter)
        {
            int? statusID = null, typeID = null, pointRouteStatusID = null;

            if (filter.Status.HasValue)
                statusID = (int)filter.Status.Value;

            if (filter.Type.HasValue)
                typeID = (int)filter.Type.Value;

            if (filter.PointRouteStatus.HasValue)
                pointRouteStatusID = (int)filter.PointRouteStatus.Value;

            return context.vw_Points2.Where(x =>
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
                                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID) &&
                                (filter.CompanyID == null || filter.AssignedCompanyID == x.AssignedCompanyID) &&
                                (filter.AssignedCompanyID == null || filter.AssignedCompanyID == x.AssignedCompanyID) &&
                                (pointRouteStatusID == null || pointRouteStatusID == x.PointRouteStatusID)
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

        public PointDetailedContract GetDetailed(Guid deviceID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return context.vw_PointsDetailed2.Where(x =>                    
                    x.DeviceID == deviceID                     
                ).FirstOrDefault().ToContract();
            }
        }

        public PointContract GetPointByDeviceID(Guid deviceID)
        {
            using (var context = new SmartWasteDatabaseConnection())
            {
                return context.vw_Points2.FirstOrDefault(x => x.DeviceID == deviceID).ToContract();
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
