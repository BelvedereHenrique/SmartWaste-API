using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RoutePointContract
    {
        public Guid ID { get; set; }
        public bool? IsCollected { get; set; }
        public Guid? CollectedBy { get; set; }
        public DateTime? CollectedOn { get; set; }
        public string Reason { get; set; }
        public Point.PointDetailedContract Point { get; set; }
    }
}