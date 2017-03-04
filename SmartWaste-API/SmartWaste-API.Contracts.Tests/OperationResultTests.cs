using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmarteWaste_API.Contracts.OperationResult;
using System.Linq;

namespace SmartWaste_API.Contracts.Tests
{
    [TestClass]
    public class OperationResultTests
    {
        [TestMethod]
        public void OperationSuccessTest()
        {
            var warning = Guid.NewGuid().ToString();

            var result = new OperationResult<bool>(true);
            result.AddWarning(warning);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Result);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => !x.IsError && x.Message == warning));
            Assert.AreEqual(result.GetMessage(true), String.Empty);
            Assert.AreEqual(result.GetMessage(false), warning);
            Assert.AreEqual(result.GetMessage(), warning);
        }

        [TestMethod]
        public void OperationFailTest()
        {
            var error = Guid.NewGuid().ToString();

            var result = new OperationResult<bool>();
            result.Result = true;
            result.AddError(error);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Result);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages.All(x => x.IsError && x.Message == error));
            Assert.AreEqual(result.GetMessage(true), error);
            Assert.AreEqual(result.GetMessage(false), String.Empty);
            Assert.AreEqual(result.GetMessage(), error);
        }
    }
}
