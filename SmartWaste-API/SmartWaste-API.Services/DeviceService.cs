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
        private readonly IDeviceHistoryRepository _deviceHistoryRepository;

        public DeviceService(
            IDeviceRepository deviceRepository,
            IPointService pointService,
            IDeviceHistoryRepository deviceHistoryRepository)
        {
            _deviceRepository = deviceRepository;
            _pointService = pointService;
            _deviceHistoryRepository = deviceHistoryRepository;
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
            _deviceHistoryRepository.Save(new DeviceHistoryContract()
            {
                Date = DateTime.Now,
                DeviceID = ID,
                ID = Guid.NewGuid(),
                PersonID = null,
                Reason = "Device activated",
                Status = DeviceStatusEnum.Activated
            });

        }

        public void Deactivate(Guid ID)
        {
            var device = _deviceRepository.Get(ID);

            if (device == null)
                throw new ArgumentException("The device does not exists or is not registered on database");

            _deviceRepository.Deactivate(ID);
            _deviceHistoryRepository.Save(new DeviceHistoryContract()
            {
                Date = DateTime.Now,
                DeviceID = ID,
                ID = Guid.NewGuid(),
                PersonID = null,
                Reason = "Device deactivated",
                Status = DeviceStatusEnum.Deactivated
            });
        }

        public void Create(DeviceContract device)
        {
            var d = _deviceRepository.Get(device.ID);
            if (d == null)
                throw new ArgumentException("Device is already registered");

            _deviceRepository.Create(device);
            _deviceHistoryRepository.Save(new DeviceHistoryContract()
            {
                Date = DateTime.Now,
                DeviceID = device.ID,
                ID = Guid.NewGuid(),
                PersonID = null,
                Reason = "Device Created",
                Status = device.Status
            });
        }

        public void SetReady(Guid deviceID)
        {
            var point = _pointService.GetPointByDeviceID(deviceID);

            var device = _deviceRepository.GetDeviceByPointID(point.ID);

            if (device.Status == DeviceStatusEnum.Deactivated)
                throw new ArgumentException("Device is deactivated and cannot be set to Ready for Collect");

            if (point == null)
                throw new ArgumentException("There is no point linked with this device");

            if (point.Status == PointStatusEnum.Full)
                return;

            point.Status = PointStatusEnum.Full;

            _pointService.Edit(point);
        }
    }
}
