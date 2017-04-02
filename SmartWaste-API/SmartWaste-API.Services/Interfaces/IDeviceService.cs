using SmarteWaste_API.Contracts.Device;
using System;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IDeviceService
    {
        DeviceContract Get(Guid ID);
        void Activate(Guid ID);
        void Deactivate(Guid ID);
        void SetReady(Guid ID);
        void Create(DeviceContract devices);
        DeviceContract Get(DeviceFilterContract filter);
        void Edit(DeviceContract device);
    }
}
