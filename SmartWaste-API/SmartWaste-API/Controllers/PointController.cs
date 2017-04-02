using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Device;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Address;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Models;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{
    public class PointController : ApiController
    {
        private readonly IPointService _pointService;
        private readonly IPersonService _personService;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IPointHistoryService _pointHistoryService;


        public PointController(IPointService pointService,
                               IPersonService personService,
                               ISecurityManager<IdentityContract> user,
                               IPointHistoryService pointHistoryService)
        {
            _pointService = pointService;
            _personService = personService;
            _user = user;
            _pointHistoryService = pointHistoryService;
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
        public IHttpActionResult GetDetailed(PointFilterContract filter)
        {
            try
            {
                var point = _pointService.GetDetailed(filter);
                if (point == null)
                    throw new NullReferenceException("Point not found.");

                var histories = _pointHistoryService.GetList(new PointHistoryFilterContract()
                {
                    PointID = point.ID
                });

                var model = new PointDetailedHistoriesModel(point, histories);

                return Ok(new JsonModel<PointDetailedHistoriesModel>(model));
            }
            catch
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was a error to retrieve the point.");
                return Ok(error);
            }
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult GetPeopleFromCompany()
        {
            try
            {
                return Ok(new JsonModel<List<PersonContract>>(_personService.GetList(new PersonFilterContract()
                {
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

        [HttpPost]
        public IHttpActionResult SetAsFullWithDevice(DeviceEventModel model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException();

                var result = _pointService.SetAsFull(new DeviceEventContract() {
                    SerialNumber = model.SerialNumber,
                    BatteryVoltage = model.BatteryVoltage,
                    ClickType = model.ClickType
                });

                return Ok(result.Success ? 1 : 0);
            }
            catch
            {
                return Ok(0);
            }
        }
        public IHttpActionResult RegisterPoint(AddressContract address)
        {
            try
            {
                _pointService.RegisterPoint(address);
                return Ok(new JsonModel<bool>(true));
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
