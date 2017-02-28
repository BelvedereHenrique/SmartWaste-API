
﻿using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Address;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Models;
using SmartWaste_API.Services;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        public AccountController(IPersonService _pService, IAddressService _aService, IAccountService _accService)
        {
            _personService = _pService;
            _addressService = _aService;
            _accountService = _accService;

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
        public IHttpActionResult SaveEnterprise(AccountEnterpriseContract enterprise)
        {
            try
            {
                var enterpriseID = _accountService.AddEnterprise(enterprise);
                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }
        [HttpPost]
        public IHttpActionResult SavePersonalProfile(PersonalSubscriptionFormContract data)
        {
            try
            {
                if (_accountService.ValidatePersonalForm(data))
                {
                    _accountService.AddPersonal(data);
                    return Ok(new JsonModel<bool>(true));
                }
                else
                {
                    return Ok(new JsonModel<string>("Invalid form subscription"));
                }
            }
            catch (Exception e)
            {
                return Ok(new JsonModel<bool>(e));
            }
        }
        [HttpGet]
        public IHttpActionResult CheckEmailAvailability(string email)
        {
            try
            {
                var availability = _accountService.CheckEmailAvailability(email);
                return Ok(new JsonModel<bool>(availability));
            }
            catch (Exception e)
            {
                return Ok(new JsonModel<bool>(e));
                throw;
            }
        }
        [HttpGet]
        public IHttpActionResult CheckCPFAvailability(string cpf)
        {
            try
            {
                var availability = _accountService.CheckCPFAvailability(cpf);
                return Ok(new JsonModel<bool>(availability));
            }
            catch (Exception e)
            {
                return Ok(new JsonModel<bool>(e));
                throw;
            }
        }

    }
}
