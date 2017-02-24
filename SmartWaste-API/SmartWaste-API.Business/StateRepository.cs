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
    public class StateRepository : IStateRepository
    {
        public List<StateContract> GetList(int CountryID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var list = context.States.Where(x => x.CountryID == CountryID).ToList().ToContract();
                return list;
            }
        }
    }
}
