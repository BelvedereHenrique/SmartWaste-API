using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Library.Security;
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
        private readonly IPersonService _personService;
        private readonly ISecurityManager<IdentityContract> _user;

        public PointController(IPointService pointService, 
                               IPersonService personService,
                               ISecurityManager<IdentityContract> user)
        {
            _pointService = pointService;
            _personService = personService;
            _user = user;
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

        [HttpPost]
        public IHttpActionResult GetDetailedList(PointFilterContract filter)
        {
            try
            {
                return Ok(new JsonModel<List<PointDetailedContract>>(_pointService.GetDetailedList(filter)));
            }
            catch (Exception ex)
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to retrieve the points.");
                return Ok(error);
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetPeopleFromCompany()
        {
            try
            {
                return Ok(new JsonModel<List<PersonContract>>(_personService.GetList(new PersonFilterContract() {
                    CompanyID = _user.User.Person.CompanyID
                })));
            }
            catch (Exception ex)
            {
                var error = new JsonModel<bool>(false);
                error.AddError(ex);
                return Ok(error);
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult SetAsFull()
        {
            try
            {
                return Ok(new JsonModel<OperationResult>(_pointService.SetAsFull()));
            }
            catch (Exception ex)
            {
                var error = new JsonModel<bool>(false);
                error.AddError(ex);
                return Ok(error);
            }
        }
    }
}
