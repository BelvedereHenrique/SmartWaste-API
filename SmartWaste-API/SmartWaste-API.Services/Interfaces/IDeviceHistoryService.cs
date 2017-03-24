using SmarteWaste_API.Contracts.Device;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IDeviceHistoryService
    {
        void Add(DeviceHistory history);
    }
}
