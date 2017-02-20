using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    internal static class StateContractParser
    {
        public static StateContract ToContract(this State state)
        {
            var contract = new StateContract()
            {
                ID = state.ID,
                Name = state.Name,
                UF = state.UF,
                CountryID = state.CountryID
            };
            return contract;
        }
        public static List<StateContract> ToContract(this List<State> list)
        {
            return list.Select(x => x.ToContract()).ToList();
        }
    }
}
