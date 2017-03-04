using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartWaste_API.Library.Tests
{
    [TestClass]
    public class MD5HelperTest
    {
        [TestMethod]
        public void CreateMD5Test()
        {
            var value = Guid.NewGuid().ToString();

            var hash = MD5Helper.Create(value);

            Assert.IsNotNull(hash);
            Assert.IsTrue(MD5Helper.Check(hash, value));
            Assert.IsFalse(MD5Helper.Check(hash, hash));
        }
    }
}
