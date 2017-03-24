using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;
using System;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;

namespace SmartWaste_API.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly ISecurityManager<IdentityContract> _user;

        public AddressService(ICountryService countryService, 
                              IStateService stateService, 
                              ICityService cityService,
                              ISecurityManager<IdentityContract> user)
        {
            this._countryService = countryService;
            this._stateService = stateService;
            this._cityService = cityService;
            this._user = user;
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
