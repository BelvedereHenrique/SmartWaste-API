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
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository _repo)
        {
            _cityRepository = _repo;
        }
        public List<CityContract> GetList(int stateID)
        {
            return _cityRepository.GetList(stateID);
        }
    }
}
