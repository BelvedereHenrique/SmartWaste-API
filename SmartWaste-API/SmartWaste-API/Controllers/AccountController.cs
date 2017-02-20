using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Models;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        public AccountController(IPersonService _pService, IAddressService _aService)
        {
            _personService = _pService;
            _addressService = _aService;
        }

        public IHttpActionResult GetCountries()
        {
            try
            {
                var countries = _addressService.GetCountryList();
                return Ok(new JsonModel<List<CountryContract>>(countries));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        public IHttpActionResult GetStates(int countryID)
        {
            try
            {
                var states = _addressService.GetStateList(countryID);
                return Ok(new JsonModel<List<StateContract>>(states));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        public IHttpActionResult GetCities(int stateID)
        {
            try
            {
                var cities = _addressService.GetCityList(stateID);
                return Ok(new JsonModel<List<CityContract>>(cities));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }
    }
}
