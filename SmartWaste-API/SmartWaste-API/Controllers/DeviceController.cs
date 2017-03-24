using SmarteWaste_API.Contracts.Device;
using SmartWaste_API.Models;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Web.Http;

namespace SmartWaste_API.Controllers
{

    public class DeviceController : ApiController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public IHttpActionResult Activate(string deviceID)
        {
            try
            {
                if (String.IsNullOrEmpty(deviceID))
                    throw new ArgumentException("Empty device ID");

                _deviceService.Activate(Guid.Parse(deviceID));

                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception)
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was an error to activate the device.");
                return Ok(error);
            }
        }
        public IHttpActionResult Deactivate(string deviceID)
        {
            try
            {
                if (String.IsNullOrEmpty(deviceID))
                    throw new ArgumentException("Empty device ID");
                _deviceService.Deactivate(Guid.Parse(deviceID));

                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception)
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was an error to deactivate the device.");
                return Ok(error);
            }
        }
        public IHttpActionResult Get(string deviceID)
        {
            try
            {
                if (String.IsNullOrEmpty(deviceID))
                    throw new ArgumentException("Empty device ID");

                var device = _deviceService.Get(Guid.Parse(deviceID));

                return Ok(new JsonModel<DeviceContract>(device));
            }
            catch (Exception)
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was an error to get the device");
                return Ok(error);
            }
        }
        
        /// <summary>
        /// This method will be called by hardware devices, such as Arduino's and DashButton's.
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public IHttpActionResult SetReady(string deviceID)
        {
            try
            {
                if (String.IsNullOrEmpty(deviceID))
                    throw new ArgumentException("Empty device ID");

                _deviceService.SetReady(Guid.Parse(deviceID));

                return Ok(new JsonModel<bool>(true));
            }
            catch (Exception)
            {
                var error = new JsonModel<bool>(false);
                error.AddError("There was an error for to activate the device.");
                return Ok(error);
            }
        }

    }

}