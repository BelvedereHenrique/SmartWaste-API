using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    internal interface IRouteHistoryInternalRepository
    {
        void Add(Data.SmartWasteDatabaseConnection context, RouteHistoryContract history);
    }
}
