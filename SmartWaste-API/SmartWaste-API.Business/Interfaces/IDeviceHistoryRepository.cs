using SmarteWaste_API.Contracts.Device;

namespace SmartWaste_API.Business.Interfaces
{

    public interface IDeviceHistoryRepository
    {
        void Save(DeviceHistoryContract deviceHistory);
    }
}
