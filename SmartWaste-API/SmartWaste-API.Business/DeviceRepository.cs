﻿using SmartWaste_API.Business.Interfaces;
using System;
using SmarteWaste_API.Contracts.Device;
using System.Linq;
using SmartWaste_API.Business.Data;
using SmartWaste_API.Business.ContractParser;

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

        public DeviceContract GetDeviceByPointID(Guid ID)
        {
            using (var context = new SmartWasteDatabaseConnection())
            {
                var point = context.Points.FirstOrDefault(x=>x.ID == ID);
                return context.Devices.FirstOrDefault(x=>x.ID == point.DeviceID).ToContract();
            }
        }

        public void Edit(DeviceContract device)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var entitie = context.Devices.Find(device.ID);

                if (entitie == null)
                    throw new ArgumentException("Device does not exist.");

                entitie.StatusID = (int)device.Status;
                entitie.TypeID = (int)device.Type;
                entitie.BatteryVoltage = device.BatteryVoltage;

                context.SaveChanges();
            }
        }

        public DeviceContract Get(DeviceFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                int? statusID = null, typeID = null;

                if (filter.Status.HasValue)
                    statusID = (int)filter.Status;

                if (filter.Type.HasValue)
                    typeID = (int)filter.Type;

                return context.Devices.Where(x =>
                    (filter.ID == null || filter.ID == x.ID) &&
                    (filter.InternalID == null || filter.InternalID == x.InternalID) &&
                    (statusID == null || statusID == x.StatusID) &&
                    (typeID == null || typeID == x.TypeID)
                ).FirstOrDefault().ToContract();
            }
        }
    }
}
