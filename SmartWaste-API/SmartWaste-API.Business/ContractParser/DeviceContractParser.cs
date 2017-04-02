using SmarteWaste_API.Contracts.Device;
using SmartWaste_API.Business.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartWaste_API.Business.ContractParser
{
    internal static class DeviceContractParser
    {
        public static DeviceContract ToContract(this Device device)
        {
            return new DeviceContract()
            {
                    ID = device.ID,
                    Status = (DeviceStatusEnum)device.StatusID,
                    InternalID = device.InternalID,
                    Type = (DeviceTypeEnum)device.TypeID,
                    BatteryVoltage = entitie.BatteryVoltage
            };
        }
        public static List<DeviceContract> ToContract(this List<Device> list)
        {
            return list.Select(x => x.ToContract()).ToList();
        }
    }
}
