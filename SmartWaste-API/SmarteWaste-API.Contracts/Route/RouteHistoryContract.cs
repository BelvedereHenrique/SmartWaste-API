using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RouteHistoryContract
    {
        public Guid ID { get; set; }
        public Guid RouteID { get; set; }
        public RouteStatusEnum Status { get; set; }
        public Guid PersonID { get; set; }
        public string Reason { get; set; }
    }
}
