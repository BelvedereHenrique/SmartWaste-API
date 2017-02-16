using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;

        public AddressService(ICountryService _cService, IStateService _sService, ICityService _ciService)
        {
            _countryService = _cService;
            _stateService = _sService;
            _cityService = _ciService;
        }

        public List<CountryContract> GetCountryList()
        {
            return _countryService.GetList();
        }

        public List<StateContract> GetStateList(int countryID)
        {
            return _stateService.GetList(countryID);
        }

        public List<CityContract> GetCityList(int stateID)
        {
            return _cityService.GetList(stateID);
        }
    }
}
