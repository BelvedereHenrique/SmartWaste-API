using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IPointRepository
    {
        void Edit(PointContract point, List<PointHistoryContract> histories);                
        List<PointContract> GetUserList(Guid personID, PointFilterContract filter);
        List<PointContract> GetCompanyList(Guid companyID, PointFilterContract filter);
        List<PointContract> GetPublicList(PointFilterContract filter);

        PointDetailedContract GetDetailed(Guid deviceID);
        List<PointDetailedContract> GetUserDetailedList(Guid personID, PointFilterContract filter);
        List<PointDetailedContract> GetCompanyDetailedList(Guid companyID, PointFilterContract filter);        

        PointContract GetPublicPoint(PointFilterContract filter);
        PointContract GetCompanyPoint(Guid companyID, PointFilterContract filter);
        PointContract GetUserPoint(Guid personID, PointFilterContract filter);

        PointDetailedContract GetCompanyDetailed(Guid companyID, PointFilterContract filter);        
        PointDetailedContract GetUserDetailed(Guid personID, PointFilterContract filter);
        PointContract GetPointByDeviceID(Guid deviceID);
        void AddCompanyPoint(PointContract contract, Guid? personID);
    }
}
