using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointHistoryFilterContract
    {
        public Guid? PointID { get; set; }
        public Guid? PersonID { get; set; }
        public Guid? CompanyID { get; set; }
    }
}
