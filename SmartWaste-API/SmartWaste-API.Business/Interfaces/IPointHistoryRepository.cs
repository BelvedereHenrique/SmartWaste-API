﻿using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IPointHistoryRepository
    {
        List<PointHistoryContract> GetList(PointHistoryFilterContract filter);
        void Add(PointHistoryContract history);
    }
}
