
using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Address;
using SmarteWaste_API.Contracts.Password;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Models;
using SmartWaste_API.Services;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        public AccountController(IPersonService _pService, IAddressService _aService, IAccountService _accService, IUserService _uService)
        {
            _personService = _pService;
            _addressService = _aService;
            _accountService = _accService;
            _userService = _uService;
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

        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserEnterprise()
        {
            try
            {
                var e = _accountService.GetUserEnterprise();
                return Ok(new JsonModel<AccountEnterpriseContract>(e));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> SaveEnterprise(AccountEnterpriseContract enterprise)
        {
            try
            {
                Task<Guid> a = _accountService.DoChangesToNewEnterprise(enterprise);
                await Task.WhenAll(a);
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
                
                throw;
            }
        }

        [HttpGet]
        public IHttpActionResult CheckUserToken(string email)
        {
            try
            {
                var result = _userService.CheckUserToken(email);
                return Ok(new JsonModel<bool>(result));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendPasswordToken(EmailModel email)
        {
            try
            {
                await _userService.SendToken(email.email);
                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        [HttpPost]
        public IHttpActionResult ChangePassword(PasswordContract password)
        {
            try
            {
                _userService.ChangePassword(password);
                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SendEmployeeEnterpriseToken(Models.EnterpriseRequestTokenModel email)
        {
            try
            {
                await _accountService.SendEmployeeEnterpriseTokenEmail(email.email, email.check);
                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult SetEnterprisePermission(Models.EnterprisePermissionModel model)
        {
            try
            {
                _accountService.SetEnterprisePermission(model.email, model.password, model.token);
                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception ex)
            {
                return Ok(new JsonModel<bool>(ex));
            }
        }
    }
}
