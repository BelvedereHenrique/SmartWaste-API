using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class CityServiceTests
    {
        [TestMethod]
        public void GeCitiesServiceListTest()
        {
            var stateID = 1;
            var list = new List<CityContract>();
            var repo = new Mock<ICityRepository>();
            repo.Setup(x => x.GetList(stateID)).Returns(list);

            var service = GetCityService(repo.Object);
            var result = service.GetList(stateID);
            Assert.AreEqual(result, list);
            repo.Verify(x => x.GetList(stateID), Times.Exactly(1));
        }

        internal ICityService GetCityService(ICityRepository _cityRepository)
        {
            return (ICityService)new CityService(_cityRepository);
        }
    }
}
