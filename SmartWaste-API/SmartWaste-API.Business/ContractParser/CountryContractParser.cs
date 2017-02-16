using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    internal static class CountryContractParser
    {
        public static CountryContract ToContract(this Country country)
        {
            var contract = new CountryContract()
            {
                ID = country.Id,
                Initials = country.Initials,
                Name = country.Name
            };
            return contract;
        }

        public static List<CountryContract> ToContract(this List<Country> list)
        {
            return list.Select(x => x.ToContract()).ToList();
        }
    }
}
