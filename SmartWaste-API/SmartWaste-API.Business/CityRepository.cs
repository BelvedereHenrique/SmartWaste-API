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
    public class CityRepository : ICityRepository
    {
        public List<CityContract> GetList(int stateID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var list = context.Cities.Where(x => x.StateID == stateID).ToList().ToContract();
                return list;
            }
        }
    }
}
