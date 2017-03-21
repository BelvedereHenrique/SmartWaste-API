using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Library.Tests;
using SmartWaste_API.Services.Security;
using SmartWaste_API.Models;
using System.Linq;

namespace SmartWaste_API.Tests.Models
{
    [TestClass]
    public class SecurityModelTest
    {
        [TestMethod]
        public void TestNotCompanyUser()
        {
            var person = SecurityManagerHelper.GetPersonContract(false);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.USER
            });

            var model = new SecurityModel(identity.Object.User);

            Assert.AreEqual(model.Name, person.Name);
            Assert.AreEqual(model.CompanyName, String.Empty);
            Assert.AreEqual(model.Roles.Count, identity.Object.User.Roles.Count);
            Assert.IsTrue(model.Roles.Any(x => identity.Object.User.Roles.Any(r => r == x)));

            Assert.IsFalse(model.CanNavigateRoutes);
            Assert.IsFalse(model.CanSaveRoutes);
            Assert.IsFalse(model.ShowRoutesMenu);
        }

        [TestMethod]
        public void TestCompanyUser()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.COMPANY_USER
            });

            var model = new SecurityModel(identity.Object.User);

            Assert.AreEqual(model.Name, person.Name);
            Assert.AreEqual(model.CompanyName, String.Empty);
            Assert.AreEqual(model.Roles.Count, identity.Object.User.Roles.Count);
            Assert.IsTrue(model.Roles.Any(x => identity.Object.User.Roles.Any(r => r == x)));

            Assert.IsTrue(model.CanNavigateRoutes);
            Assert.IsFalse(model.CanSaveRoutes);
            Assert.IsTrue(model.ShowRoutesMenu);
        }

        [TestMethod]
        public void TestCompanyRouteUser()
        {
            var person = SecurityManagerHelper.GetPersonContract(true);
            var user = SecurityManagerHelper.GetUserContract();
            var identity = SecurityManagerHelper.GetAuthenticatedIdentity(person, user, new System.Collections.Generic.List<string>() {
                RolesName.COMPANY_ROUTE                
            });

            var model = new SecurityModel(identity.Object.User);

            Assert.AreEqual(model.Name, person.Name);
            Assert.AreEqual(model.CompanyName, String.Empty);
            Assert.AreEqual(model.Roles.Count, identity.Object.User.Roles.Count);
            Assert.IsTrue(model.Roles.Any(x => identity.Object.User.Roles.Any(r => r == x)));

            Assert.IsFalse(model.CanNavigateRoutes);
            Assert.IsTrue(model.CanSaveRoutes);
            Assert.IsTrue(model.ShowRoutesMenu);
        }
    }
}
