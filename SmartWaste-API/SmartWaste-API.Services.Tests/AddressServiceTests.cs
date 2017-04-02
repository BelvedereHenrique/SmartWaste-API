using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class AddressServiceTests
    {
        [TestMethod]
        public void GetCountryListTest()
        {
            var countryList = new List<CountryContract>();
            var countryService = new Mock<ICountryService>();
            countryService.Setup(x => x.GetList()).Returns(countryList);

            var service = GetAddressService(countryService.Object, null, null,null,null);
            var result = service.GetCountryList();

            Assert.AreEqual(result, countryList);
            countryService.Verify(x => x.GetList(), Times.Exactly(1));
        }

        [TestMethod]
        public void GetStateListTest()
        {
            var countryID = 1;
            var stateList = new List<StateContract>();
            var stateService = new Mock<IStateService>();
            stateService.Setup(x => x.GetList(countryID)).Returns(stateList);

            var service = GetAddressService(null, stateService.Object, null,null,null);
            var result = service.GetStateList(countryID);

            Assert.AreEqual(result, stateList);
            stateService.Verify(x => x.GetList(countryID), Times.Exactly(1));
        }

        [TestMethod]
        public void GetCityListTest()
        {
            var stateID = 1;
            var cityList = new List<CityContract>();
            var cityService = new Mock<ICityService>();
            cityService.Setup(x => x.GetList(stateID)).Returns(cityList);

            var service = GetAddressService(null, null, cityService.Object,null,null);
            var result = service.GetCityList(stateID);

            Assert.AreEqual(result, cityList);
            cityService.Verify(x => x.GetList(stateID), Times.Exactly(1));
        }
        
        internal IAddressService GetAddressService(ICountryService _countryService, IStateService _stateService, ICityService _cityService, IAddressRepository _addressRepository, IAccountService _accountService)
        {
            return (IAddressService)new AddressService(_countryService, _stateService, _cityService, _accountService, _addressRepository);
        }
    }                              
}                                  
