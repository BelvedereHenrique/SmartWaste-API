using SmarteWaste_API.Contracts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class RoleContractParser
    {
        public static RoleContract ToContract(this Data.Role entitie)
        {
            if (entitie == null) return null;

            return new RoleContract() {
                ID = entitie.ID,
                Name = entitie.Name
            };
        }

        public static List<RoleContract> ToContracts(this List<Data.Role> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }

        public static Data.Role ToEntitie(this RoleContract entitie)
        {
            if (entitie == null) return null;

            return new Data.Role()
            {
                ID = entitie.ID,
                Name = entitie.Name
            };
        }

        public static List<Data.Role> ToEntities(this List<RoleContract>  contracts)
        {
            return contracts.Select(x => x.ToEntitie()).ToList();
        }
    }
}
