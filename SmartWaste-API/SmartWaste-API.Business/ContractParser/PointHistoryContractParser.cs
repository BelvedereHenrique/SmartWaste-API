using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class PointHistoryContractParser
    {
        public static PointHistoryContract ToContract(this Data.PointHistory entitie)
        {
            if (entitie == null) return null;
            return new PointHistoryContract() {
                Date = entitie.Date,
                ID = entitie.ID,
                Person = entitie.Person.ToContract(),
                PointID = entitie.PointID,
                Reason = entitie.Reason,
                Status = (PointStatusEnum)entitie.StatusID
            };
        }

        public static List<PointHistoryContract> ToContracts(this List<Data.PointHistory> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }

        public static Data.PointHistory ToEntitie(this PointHistoryContract contract)
        {
            return new Data.PointHistory()
            {
                Date = contract.Date,
                ID = contract.ID,
                PersonID = contract.Person.ID,
                PointID = contract.PointID,
                Reason = contract.Reason,
                StatusID = (int)contract.Status
            };
        }
    }
}
