using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RouteDetailedContract : RouteContract
    {
        public RouteDetailedContract() {
            this.RoutePoints = new List<RoutePointContract>();
            this.Histories = new List<RouteHistoryContract>();
        }
                
        public List<RoutePointContract> RoutePoints { get; set; }
        public List<RouteHistoryContract> Histories { get; set; }        
    }
}
