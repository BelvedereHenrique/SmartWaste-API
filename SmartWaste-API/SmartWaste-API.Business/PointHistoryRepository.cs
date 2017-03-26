using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Business.Data;
using SmartWaste_API.Business.ContractParser;

namespace SmartWaste_API.Business
{
    public class PointHistoryRepository : IPointHistoryRepository, IPointHistoryInternalRepository
    {
        public List<PointHistoryContract> GetList(PointHistoryFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return context.PointHistories.Include("Person").Where(x =>
                    (filter.PointID == null || filter.PointID == x.PointID) &&
                    (filter.PersonID == null || filter.PersonID == x.PersonID) &&
                    (filter.CompanyID == null || filter.CompanyID == x.Person.CompanyID)
                ).ToList().ToContracts();
            }
        }

        public void Add(PointHistoryContract history)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                ((IPointHistoryInternalRepository)this).Add(context, history);

                context.SaveChanges();
            }
        }

        void IPointHistoryInternalRepository.Add(SmartWasteDatabaseConnection context, PointHistoryContract history)
        {
            context.PointHistories.Add(history.ToEntitie());
        }
    }
}
