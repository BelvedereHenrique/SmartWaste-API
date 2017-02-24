﻿using SmarteWaste_API.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IUserRepository
    {
        UserContract Get(UserFilterContract filter);
        List<UserContract> GetList(UserFilterContract filter);
    }
}