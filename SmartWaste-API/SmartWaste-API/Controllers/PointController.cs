using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Models;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class PointController : ApiController
    {
        private readonly IPointService _pointService;

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        [HttpPost]
        public IHttpActionResult GetList(PointFilterContract filter)
        {
            try
            {
                return Ok(new JsonModel<List<PointContract>>(_pointService.GetList(filter)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to retrieve the points.");
                return Ok(error);
            }            
        }
    }
}
