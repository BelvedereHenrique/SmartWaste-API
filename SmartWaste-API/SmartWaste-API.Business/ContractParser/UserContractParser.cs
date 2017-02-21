using SmarteWaste_API.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWaste_API.Business.Data;

namespace SmartWaste_API.Business.ContractParser
{
    public static class UserContractParser
    {
        public static UserContract ToContract(this User entitie)
        {
            if (entitie == null) return null;

            return new UserContract()
            {
                ExpirationDate = entitie.ExpirationDate,
                ID = entitie.ID,
                Login = entitie.Login,
                Password = entitie.Password,
                RecoveredOn = entitie.RecoveredOn,
                RecoveryToken = entitie.RecoveryToken,
                Roles = entitie.UserRoles.Select(x => x.Role.ToContract()).ToList()
            };
        }

        public static List<UserContract> ToContracts(this List<User> entities)
        {
            return entities.Select(entitie => entitie.ToContract()).ToList();
        }

        public static User ToEntitie(this UserContract contract)
        {
            if (contract == null) return null;

            return new User() {
                ExpirationDate = contract.ExpirationDate,
                ID = contract.ID,
                Login = contract.Login,
                Password = contract.Password,
                RecoveredOn = contract.RecoveredOn,
                RecoveryToken = contract.RecoveryToken
            };
        }

        public static List<User> ToEntities(this List<UserContract> contracts)
        {
            return contracts.Select(contract => contract.ToEntitie()).ToList();
        }
    }
}
