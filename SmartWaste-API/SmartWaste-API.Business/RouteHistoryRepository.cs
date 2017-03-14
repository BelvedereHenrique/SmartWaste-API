using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Route;
using SmartWaste_API.Business.Data;
using SmartWaste_API.Business.ContractParser;

namespace SmartWaste_API.Business
{
    public class RouteHistoryRepository : IRouteHistoryRepository, IRouteHistoryInternalRepository
    {
        public void Add(RouteHistoryContract history)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                ((IRouteHistoryInternalRepository)this).Add(context, history);
            }            
        }

        void IRouteHistoryInternalRepository.Add(SmartWasteDatabaseConnection context, RouteHistoryContract history)
        {
            context.RouteHistories.Add(history.ToEntitie());
        }
    }
}
