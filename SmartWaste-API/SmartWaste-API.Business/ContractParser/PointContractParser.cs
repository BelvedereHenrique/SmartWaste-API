using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class PointContractParser
    {
        public static PointContract ToContract(this Data.vw_GetPoints entitie)
        {
            if (entitie == null) return null;

            return new PointContract()
            {
                ID = entitie.ID,
                Status = (PointStatusEnum)entitie.StatusID,
                Type = (PointTypeEnum)entitie.TypeID,                                    
                Latitude = entitie.Latitude,                    
                Longitude = entitie.Longitude,    
                DeviceID = entitie.DeviceID,
                PersonID = entitie.PersonID,
                UserID = entitie.UserID                
            };
        }

        public static List<PointContract> ToContracts(this List<Data.vw_GetPoints> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }
    }
}
