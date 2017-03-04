using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Library.Tests
{
    [TestClass]
    public class ClaimParserTest
    {
        [TestMethod]
        public void ClaimsParserTest()
        {
            var model = new IdentityContract() {
                Login = Guid.NewGuid().ToString(),
                IsAuthenticated = true,
                AuthenticationType = Guid.NewGuid().ToString()
            };

            var claims = ClaimsParser.Create<IdentityContract>(model);

            Assert.AreEqual(claims.Claims.Count(), 2);
            Assert.AreEqual(claims.Claims.First(x => x.Type == "sub").Value, model.Login);
            Assert.AreEqual(claims.Claims.First(x => x.Type == "data").Value, JsonConvert.SerializeObject(model));

            var parsedIdentity = ClaimsParser.Parse<IdentityContract>(claims);

            Assert.AreEqual(model.AuthenticationType, parsedIdentity.AuthenticationType);
            Assert.AreEqual(model.Login, parsedIdentity.Login);
            Assert.AreEqual(model.Login, parsedIdentity.Login);
        }
    }
}