using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository _repo)
        {
            _stateRepository = _repo;
        }
        public List<StateContract> GetList(int CountryID)
        {
            return _stateRepository.GetList(CountryID);
        }
    }

}
