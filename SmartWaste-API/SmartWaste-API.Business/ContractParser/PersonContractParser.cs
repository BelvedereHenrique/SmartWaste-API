using SmarteWaste_API.Contracts.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class PersonContractParser
    {
        public static PersonContract ToContract(this Data.Person entitie)
        {
            if (entitie == null) return null;

            return new PersonContract()
            {
                ID = entitie.ID,
                CompanyID = entitie.CompanyID,
                Email = entitie.Email,
                Name = entitie.Name,
                PersonType = (PersonTypeEnum)entitie.PersonTypeID,
                UserID = entitie.UserID
            };
        }

        public static List<PersonContract> ToContracts(this List<Data.Person> entities)
        {
            return entities.Select(entitie => entitie.ToContract()).ToList();
        }

        public static Data.Person ToEntitie(this PersonContract contract)
        {
            if (contract == null) return null;

            return new Data.Person()
            {
                ID = contract.ID,
                CompanyID = contract.CompanyID,
                Email = contract.Email,
                Name = contract.Name,
                PersonTypeID = (int)contract.PersonType,
                UserID = contract.UserID
            };
        }

        public static List<Data.Person> ToEntities(this List<PersonContract> contracts)
        {
            return contracts.Select(contract => contract.ToEntitie()).ToList();
        }
    }
}
