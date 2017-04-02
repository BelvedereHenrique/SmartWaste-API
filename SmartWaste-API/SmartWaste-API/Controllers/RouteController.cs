using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Route;
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
    public class RouteController : ApiController
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            this._routeService = routeService;
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult GetDetailed(RouteFilterContract filter)
        {
            try
            {
                return Ok(new JsonModel<RouteDetailedContract>(_routeService.GetDetailed(filter)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to get the route.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult GetDetailedList(RouteFilterContract filter)
        {
            try
            {
                return Ok(new JsonModel<List<RouteDetailedContract>>(_routeService.GetDetailedList(filter)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to get the routes.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult GetList(RouteGetListModel model)
        {
            try
            {
                return Ok(new JsonModel<List<RouteContract>>(_routeService.GetList(model.Status)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to get the routes.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]        
        public IHttpActionResult Create(RouteAddModel route)

        {
            try
            {
                return Ok(new JsonModel<OperationResult<Guid>>(_routeService.Create(route.AssignedToID, route.PointIDs, route.ExpectedKilometers, route.ExpectedMinutes)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to create the route.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult Recreate(RouteAddModel route)
        {
            try
            {
                return Ok(new JsonModel<OperationResult<Guid>>(_routeService.Recreate(route.RouteID.Value, route.AssignedToID, route.PointIDs, route.ExpectedKilometers, route.ExpectedMinutes)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to create the route.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult Disable(RouteDisableModel route)
        {
            try
            {
                return Ok(new JsonModel<OperationResult>(_routeService.Disable(route.RouteID)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to create the route.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult StartNavigation(RouteNavigationModel route)
        {
            try
            {
                return Ok(new JsonModel<OperationResult<RouteDetailedContract>>(_routeService.StartNavigation(route.RouteID)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to navigate the route.");
                return Ok(error);
            }
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult CollectPoint(RouteNavigationCollectPointModel data)
        {
            try
            {
                return Ok(new JsonModel<OperationResult>(_routeService.CollectPoint(data.RouteID, data.PointID, data.Collected, data.Reason)));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to collect the point.");
                return Ok(error);
            }
        }
    }
}
