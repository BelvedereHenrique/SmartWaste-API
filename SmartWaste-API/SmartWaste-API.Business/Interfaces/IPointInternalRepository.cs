using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    internal interface IPointInternalRepository
    {
        void Edit(Data.SmartWasteDatabaseConnection context, PointContract point, List<PointHistoryContract> histories);
        void Edit(Data.SmartWasteDatabaseConnection context, List<PointContract> points, List<PointHistoryContract> histories);
        void Edit(Data.SmartWasteDatabaseConnection context, List<PointDetailedContract> points, List<PointHistoryContract> histories);
    }
}
