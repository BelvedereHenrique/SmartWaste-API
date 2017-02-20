using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    internal static class CityContractParser
    {
        public static CityContract ToContract(this City city)
        {
            var contract = new CityContract()
            {
                ID = city.ID,
                Name = city.Name,
                StateID = city.StateID
            };
            return contract;
        }
        public static List<CityContract> ToContract(this List<City> list)
        {
            return list.Select(x => x.ToContract()).ToList();
        }
    }
}
