using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWaste_API.Library.Tests;
using SmartWaste_API.Services.Security;
using SmartWaste_API.Controllers;
using System.Web.Http.Results;
using SmartWaste_API.Models;

namespace SmartWaste_API.Tests
{
    [TestClass]
    public class SecurityControllerTests
    {
        [TestMethod]
        public void GetUserInfoSuccessfullTest()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.USER
            });

            var controller = new SecurityController(identity.Object);
            var result = (controller.GetUserInfo() as OkNegotiatedContentResult<JsonModel<SecurityModel>>).Content;

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Messages.Count, 0);
            Assert.IsNotNull(result.Result);
        }
    }
}
