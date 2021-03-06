﻿using SmarteWaste_API.Contracts.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IAddressRepository
    {
        AddressContract GetAddress(Guid addressID);
        void Add(AddressContract contract);
    }
}
