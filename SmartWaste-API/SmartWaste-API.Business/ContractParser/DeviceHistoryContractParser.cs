using SmarteWaste_API.Contracts.Device;
using SmartWaste_API.Business.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartWaste_API.Business.ContractParser
{
    internal static class DeviceHistoryContractParser
    {
        public static DeviceHistoryContract ToContract(this DeviceHistory deviceHistory)
        {
            return new DeviceHistoryContract()
            {
                Date = deviceHistory.Date,
                DeviceID = deviceHistory.DeviceID,
                ID = deviceHistory.ID,
                PersonID = deviceHistory.PersonID,
                Reason = deviceHistory.Reason,
                Status = (DeviceStatusEnum)deviceHistory.StatusID
            };
        }
        public static List<DeviceHistoryContract> ToContracts(this List<DeviceHistory> deviceHistoryList)
        {
            return deviceHistoryList.Select(x => x.ToContract()).ToList();
        }
    }
}
