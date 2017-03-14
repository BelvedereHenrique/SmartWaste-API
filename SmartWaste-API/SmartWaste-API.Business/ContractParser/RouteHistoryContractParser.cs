using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class RouteHistoryContractParser
    {
        public static RouteHistoryContract ToContract(this Data.RouteHistory entitie)
        {
            if (entitie == null) return null;

            return new RouteHistoryContract()
            {
                ID = entitie.ID,
                PersonID = entitie.PersonID,
                Reason = entitie.Reason,
                RouteID = entitie.RouteID,
                Status = (RouteStatusEnum)entitie.StatusID
            };
        }

        public static List<RouteHistoryContract> ToContracts(this List<Data.RouteHistory> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }

        public static Data.RouteHistory ToEntitie(this RouteHistoryContract contract)
        {
            if (contract == null) return null;

            return new Data.RouteHistory() {
                ID = contract.ID,
                PersonID = contract.PersonID,
                Reason = contract.Reason,
                RouteID = contract.RouteID,
                StatusID = (int)contract.Status                
            };
        }
    }
}
