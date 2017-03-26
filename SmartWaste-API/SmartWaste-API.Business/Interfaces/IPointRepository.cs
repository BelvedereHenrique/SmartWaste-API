using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IPointRepository
    {
        void Edit(PointContract point, List<PointHistoryContract> histories);
        List<PointContract> GetList(PointFilterContract filter);
        List<PointDetailedContract> GetDetailedList(PointFilterContract filter);
        PointDetailedContract GetDetailed(PointFilterContract filter);
        PointContract GetPointByDeviceID(Guid deviceID);
        void AddCompanyPoint(PointContract contract, Guid? personID);

    }
}
