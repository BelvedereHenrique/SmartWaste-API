using SmarteWaste_API.Contracts.Device;
using System;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IDeviceRepository
    {
        DeviceContract Get(Guid ID);
        
        void Activate(Guid ID);
        void Deactivate(Guid ID);
        void Create(DeviceContract devices);
    }
}
