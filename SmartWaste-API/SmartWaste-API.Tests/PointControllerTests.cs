using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmartWaste_API.Services.Interfaces;
using SmarteWaste_API.Contracts.Point;
using System.Collections.Generic;
using SmartWaste_API.Controllers;
using SmartWaste_API.Models;
using System.Web.Http.Results;
using System.Linq;

namespace SmartWaste_API.Tests
{
    [TestClass]
    public class PointControllerTests
    {
        [TestMethod]
        public void GetListSuccessTest()
        {
            var filter = new PointFilterContract();
            var points = new List<PointContract>();

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Returns(points);

            var controller = new PointController(pointService.Object);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<List<PointContract>>>;

            Assert.IsTrue(jsonModel.Content.Success);
            Assert.AreEqual(jsonModel.Content.Result, points);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 0);
        }

        [TestMethod]
        public void GetListFailTest()
        {
            var filter = new PointFilterContract();
            var points = new List<PointContract>();

            var pointService = new Mock<IPointService>();
            pointService.Setup(x => x.GetList(It.IsAny<PointFilterContract>())).Throws(new Exception());

            var controller = new PointController(pointService.Object);
            var jsonModel = controller.GetList(filter) as OkNegotiatedContentResult<JsonModel<bool>>;

            Assert.IsFalse(jsonModel.Content.Success);
            Assert.IsFalse(jsonModel.Content.Result);
            Assert.AreEqual(jsonModel.Content.Messages.Count, 1);
            Assert.IsTrue(jsonModel.Content.Messages.All(x => x.IsError && !String.IsNullOrEmpty(x.Message)));
        }
    }
}
