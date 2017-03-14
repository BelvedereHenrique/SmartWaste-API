using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Services.Interfaces;
using System.Collections.Generic;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class CountryServiceTests
    {

        [TestMethod]
        public void GetCountryServiceListTest()
        {
            var list = new List<CountryContract>();
            var repo = new Mock<ICountryRepository>();
            repo.Setup(x => x.GetList()).Returns(list);

            var service = GetCountryService(repo.Object);
            var result = service.GetList();
            Assert.AreEqual(result, list);
            repo.Verify(x => x.GetList(), Times.Exactly(1));
        }

        internal ICountryService GetCountryService(ICountryRepository _countryRepository)
        {
            return (ICountryService)new CountryService(_countryRepository);
        }
    }
}
