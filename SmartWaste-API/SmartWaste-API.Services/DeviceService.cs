using System;
using SmarteWaste_API.Contracts.Device;
using SmartWaste_API.Services.Interfaces;
using SmartWaste_API.Business.Interfaces;
using System.Linq;
using SmarteWaste_API.Contracts.Point;

namespace SmartWaste_API.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IPointService _pointService;

        public DeviceService(
            IDeviceRepository deviceRepository, 
            IPointService pointService)
        {
            _deviceRepository = deviceRepository;
            _pointService = pointService;
        }

        public DeviceContract Get(Guid ID)
        {
            return _deviceRepository.Get(ID);
        }

        public void Activate(Guid ID)
        {
            var device = _deviceRepository.Get(ID);

            if (device == null)
                throw new ArgumentException("The device does not exists or is not registered on database");

            _deviceRepository.Activate(ID);

        }

        public void Deactivate(Guid ID)
        {
            var device = _deviceRepository.Get(ID);

            if (device == null)
                throw new ArgumentException("The device does not exists or is not registered on database");

            _deviceRepository.Deactivate(ID);
        }

        public void Create(DeviceContract device)
        {
           var d = _deviceRepository.Get(device.ID);
            if (d == null)
                throw new ArgumentException("Device is already registered");

            _deviceRepository.Create(device);

        }

        public void SetReady(Guid ID)
        {
            var point = _pointService.GetList(new PointFilterContract() { DeviceID = ID}).SingleOrDefault();

            if (point.Status == PointStatusEnum.Full)
                return;

            point.Status = PointStatusEnum.Full;

            _pointService.Edit(point);

        }
    }
}
