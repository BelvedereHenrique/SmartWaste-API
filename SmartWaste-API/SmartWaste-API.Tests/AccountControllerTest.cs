using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWaste_API.Controllers;
using SmartWaste_API.Services.Interfaces;
using Moq;
using System.Collections.Generic;
using SmarteWaste_API.Contracts.Address;
using System.Web.Http.Results;
using SmartWaste_API.Models;
using SmarteWaste_API.Contracts.Account;
using System.Threading.Tasks;

namespace SmartWaste_API.Tests
{
    [TestClass]
    public class AccountControllerTest
    {
        private const int MOCKED_STATE_ID = 1;
        private const int MOCKED_COUNTRY_ID = 1;

        [TestMethod]
        public void GetCountriesTest_Success()
        {
            var listCountry = new List<CountryContract>();
            var addressService = new Mock<IAddressService>();
            addressService.Setup(x => x.GetCountryList()).Returns(listCountry);

            var controller = GetAccountController(null, addressService.Object, null,null);
            var jsonModel = controller.GetCountries() as OkNegotiatedContentResult<JsonModel<List<CountryContract>>>;
            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, listCountry);
            addressService.Verify(x => x.GetCountryList(), Times.Exactly(1));

        }
       [TestMethod]
        public void GetCountriesTest_Fail()
        {
            var addressService = new Mock<IAddressService>();
            addressService.Setup(x => x.GetCountryList()).Throws(new Exception());

            var controller = GetAccountController(null, addressService.Object, null,null);
            var jsonModel = controller.GetCountries() as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
        }

        [TestMethod]
        public void GetStatesTest_Success()
        {
            var listStates = new List<StateContract>();

            var addressService = new Mock<IAddressService>();
            addressService.Setup(x => x.GetStateList(MOCKED_COUNTRY_ID)).Returns(listStates);

            var controller = GetAccountController(null, addressService.Object, null,null);
            var jsonModel = controller.GetStates(MOCKED_COUNTRY_ID) as OkNegotiatedContentResult<JsonModel<List<StateContract>>>;
            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, listStates);
            addressService.Verify(x => x.GetStateList(MOCKED_COUNTRY_ID), Times.Exactly(1));
        }

        [TestMethod]
        public void GetStatesTest_Fail()
        {
            var addressService = new Mock<IAddressService>();
            addressService.Setup(x => x.GetStateList(MOCKED_COUNTRY_ID)).Throws(new Exception());

            var controller = GetAccountController(null, addressService.Object, null,null);
            var jsonModel = controller.GetStates(MOCKED_COUNTRY_ID) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
        }

         [TestMethod]
         public void GetCitiesTest_Success()
         {
            var listCitites = new List<CityContract>();

            var addressService = new Mock<IAddressService>();
            addressService.Setup(x => x.GetCityList(MOCKED_STATE_ID)).Returns(listCitites);

            var controller = GetAccountController(null, addressService.Object, null,null);
            var jsonModel = controller.GetCities(MOCKED_STATE_ID) as OkNegotiatedContentResult<JsonModel<List<CityContract>>>;
            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, listCitites);
            addressService.Verify(x => x.GetCityList(MOCKED_STATE_ID), Times.Exactly(1));
         }

         [TestMethod]
         public void GeCitiesTest_Fail()
         {
             var addressService = new Mock<IAddressService>();
             addressService.Setup(x => x.GetCityList(MOCKED_STATE_ID)).Throws(new Exception());

             var controller = GetAccountController(null, addressService.Object, null,null);
             var jsonModel = controller.GetCities(MOCKED_STATE_ID) as OkNegotiatedContentResult<JsonModel<bool>>;

             Assert.IsFalse(jsonModel.Content.Success);
             Assert.IsFalse(jsonModel.Content.Result);
             Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
         }

         [TestMethod]
         public void GetUserEnterpriseTest_Success()
         {
            var accountUserEnterprise = new AccountEnterpriseContract();
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserEnterprise()).Returns(accountUserEnterprise);

            var controller = GetAccountController(null, null, accountService.Object,null);
            var jsonModel = controller.GetUserEnterprise() as OkNegotiatedContentResult<JsonModel<AccountEnterpriseContract>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, accountUserEnterprise);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            accountService.Verify(x => x.GetUserEnterprise(), Times.Exactly(1));
         }


         [TestMethod]
         public void GetUserEnterpriseTest_Fail()
         {
            var accountUserEnterprise = new AccountEnterpriseContract();
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserEnterprise()).Throws(new Exception());

            var controller = GetAccountController(null, null, accountService.Object,null);
            var jsonModel = controller.GetUserEnterprise() as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
        }
         [TestMethod]
         public async Task SaveEnterpriseTest_Success()
         {
            var enterprise = new AccountEnterpriseContract() { ID = Guid.NewGuid() };
            var responseTask = Task.FromResult(enterprise.ID.Value);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.DoChangesToNewEnterprise(enterprise)).ReturnsAsync(enterprise.ID.Value);

            var controller = GetAccountController(null, null, accountService.Object,null);
            var jsonModel = await controller.SaveEnterprise(enterprise) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, true);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
            accountService.Verify(x => x.DoChangesToNewEnterprise(enterprise), Times.Exactly(1));
        }

         [TestMethod]
         public async Task SaveEnterpriseTest_Fail()
         {
            var enterprise = new AccountEnterpriseContract() { ID = Guid.NewGuid() };

            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.DoChangesToNewEnterprise(enterprise)).Throws(new Exception());

            var controller = GetAccountController(null, null, accountService.Object,null);
            var jsonModel = await controller.SaveEnterprise(enterprise) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
        }
        
        internal AccountController GetAccountController(IPersonService _personService,IAddressService _addressService, IAccountService _accountService, IUserService _userService)
        {
            return new AccountController(_personService, _addressService, _accountService,_userService);
        }
    }
}
