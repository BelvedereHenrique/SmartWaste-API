using SmarteWaste_API.Contracts.Device;
using SmarteWaste_API.Contracts.OperationResult;
﻿using SmarteWaste_API.Contracts.Address;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IPointService
    {
        List<PointContract> GetList(PointFilterContract filter);
        List<PointDetailedContract> GetDetailedList(PointFilterContract filter);
        PointDetailedContract GetDetailed(PointFilterContract filter);
        OperationResult SetAsFull();
        void Edit(PointContract point);
        PointContract GetPointByDeviceID(Guid deviceID);
        void RegisterPoint(AddressContract address);
        OperationResult SetAsFull(DeviceEventContract device);
    }
}
