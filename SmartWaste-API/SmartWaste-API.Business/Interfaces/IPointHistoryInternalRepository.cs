using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    internal interface IPointHistoryInternalRepository
    {
        void Add(Data.SmartWasteDatabaseConnection context, PointHistoryContract history);
    }
}
