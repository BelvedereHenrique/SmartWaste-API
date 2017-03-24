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
