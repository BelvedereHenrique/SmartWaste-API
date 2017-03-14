using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;

namespace SmartWaste_API.Services.Tests
{
    [TestClass]
    public class PersonServiceTests
    {
        [TestMethod]
        public void GetPersonServiceTest()
        {
            var person = new PersonContract();
            var filter = new PersonFilterContract();
            var repo = new Mock<IPersonRepository>();
            repo.Setup(x => x.Get(filter)).Returns(person);

            var service = GetPersonService(repo.Object);
            var result = service.Get(filter);

            Assert.AreEqual(result, person);
            repo.Verify(x => x.Get(filter), Times.Exactly(1));
        }

        [TestMethod]
        public void SetCompanyIDTest()
        {
            Guid companyID = Guid.NewGuid();
            var filter = new PersonFilterContract();

            var repo = new Mock<IPersonRepository>();
            repo.Setup(x => x.SetCompanyID(companyID, filter));

            var service = GetPersonService(repo.Object);
            service.SetCompanyID(companyID, filter);

            repo.Verify(x => x.SetCompanyID(companyID, filter), Times.Exactly(1));
        }

        internal IPersonService GetPersonService(IPersonRepository _personRepository)
        {
            return (IPersonService)new PersonService(_personRepository);
        }
    }
}
