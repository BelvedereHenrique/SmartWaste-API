﻿using SmarteWaste_API.Contracts.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface ICityService
    {
        List<CityContract> GetList(int stateID);
    }
}
