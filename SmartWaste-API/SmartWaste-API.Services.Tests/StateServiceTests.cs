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
    public class StateServiceTests
    {
        [TestMethod]
        public void GeStateServiceListTest()
        {
            var countryID = 1;
            var list = new List<StateContract>();
            var repo = new Mock<IStateRepository>();
            repo.Setup(x => x.GetList(countryID)).Returns(list);

            var service = GetStateService(repo.Object);
            var result = service.GetList(countryID);
            Assert.AreEqual(result, list);
            repo.Verify(x => x.GetList(countryID), Times.Exactly(1));
        }

        internal IStateService GetStateService(IStateRepository _staeRepository)
        {
            return (IStateService)new StateService(_staeRepository);
        }
    }
}
