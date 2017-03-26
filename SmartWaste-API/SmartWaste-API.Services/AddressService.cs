using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;
using System;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Business.Interfaces;

namespace SmartWaste_API.Services
{
    public class AddressService : IAddressService
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IAccountService _accountService;
        private readonly IAddressRepository _addressRepository;

        public AddressService(ICountryService countryService, 
                              IStateService stateService, 
                              ICityService cityService,
                              IAccountService accountService,
                              IAddressRepository repository
                              )
        {
            this._countryService = countryService;
            this._stateService = stateService;
            this._cityService = cityService;
            _accountService = accountService;
            _addressRepository = repository;
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

        public AddressContract GetAddress(Guid address)
        {
            throw new NotImplementedException();
        }

        public void Add(AddressContract address)
        {
            _addressRepository.Add(address);
        }
    }
}
