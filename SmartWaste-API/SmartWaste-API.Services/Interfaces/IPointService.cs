using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IPointService
    {
        List<PointContract> GetList(PointFilterContract filter);
        List<PointDetailedContract> GetDetailedList(PointFilterContract filter);
        PointDetailedContract GetDetailed(PointFilterContract filter);
        OperationResult SetAsFull();
        void Edit(PointContract point);
    }
}
