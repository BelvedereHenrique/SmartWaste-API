using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Extensions;
using SmartWaste_API.Models;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartWaste_API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        public AccountController(IPersonService _pService, IAddressService _aService)
        {
            _personService = _pService;
            _addressService = _aService;
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestEnterpriseAccount()
        {
            //TODO: passar pela tabela de requests. pendente com o mesmo userID, nao prossegir.
            var userId = Guid.Parse("ed270ec2-f52a-4321-b807-0ad204ed3072");
            var result = _personService.GetByUserID(userId);
            return View();
        }
        [HttpGet]
        [AllowCrossSiteJson]
        public JsonResult GetCountries()
        {
            try
            {
                var countries = _addressService.GetCountryList();
                return Json(new JsonModel<List<CountryContract>>(countries),JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel<bool>(ex), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetStates(int countryID)
        {
            try
            {
                var states = _addressService.GetStateList(countryID);
                return Json(new JsonModel<List<StateContract>>(states), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel<bool>(ex), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCities(int stateID)
        {
            try
            {
                var cities = _addressService.GetCityList(stateID);
                return Json(new JsonModel<List<CityContract>>(cities), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonModel<bool>(ex), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
