using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.ContractParser;
using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business
{
    public class CountryRepository : ICountryRepository
    {
        public List<CountryContract> GetList()
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var list = context.Country.ToList().ToContract();
                return list;
            }
        }
    }
}
