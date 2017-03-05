using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IPointRepository
    {
        void Edit(PointContract point);
        List<PointContract> GetList(PointFilterContract filter);
        List<PointDetailedContract> GetDetailedList(PointFilterContract filter);
    }
}
