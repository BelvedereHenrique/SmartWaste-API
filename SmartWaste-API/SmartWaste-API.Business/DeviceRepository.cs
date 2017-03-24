using SmartWaste_API.Business.Interfaces;
using System;
using SmarteWaste_API.Contracts.Device;
using System.Linq;
using SmartWaste_API.Business.Data;

namespace SmartWaste_API.Business
{
    public class DeviceRepository : IDeviceRepository
    {
        public void Activate(Guid deviceID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var device = context.Devices.SingleOrDefault(x => x.ID == deviceID);
                device.StatusID = (int)DeviceStatusEnum.Activated;
                context.SaveChanges();
            }
        }
        public void Deactivate(Guid deviceID)
        {

            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var device = context.Devices.SingleOrDefault(x => x.ID == deviceID);
                device.StatusID = (int)DeviceStatusEnum.Deactivated;
                context.SaveChanges(); 
            }
        }

        public void Create(DeviceContract device)
        {
            var newDevice = new Device()
            {
                ID = device.ID,
                StatusID = (int)device.Status,
                TypeID = (int)device.Type,
                InternalID = device.InternalID,
                
            };
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                context.Devices.Add(newDevice);
                context.SaveChanges();
            }
        }

        public DeviceContract Get(Guid ID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var device = context.Devices.SingleOrDefault(x=>x.ID == ID);
                return new DeviceContract() {
                    ID = device.ID,
                    InternalID = device.InternalID,
                    Status = (DeviceStatusEnum)device.StatusID,
                    Type = (DeviceTypeEnum)device.TypeID
                };
            }
        }
    }
}
