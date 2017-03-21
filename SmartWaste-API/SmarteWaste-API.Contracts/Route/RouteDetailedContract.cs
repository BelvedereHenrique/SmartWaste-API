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
            this.Points = new List<Point.PointDetailedContract>();
            this.Histories = new List<RouteHistoryContract>();
        }
                
        public List<Point.PointDetailedContract> Points { get; set; }
        public List<RouteHistoryContract> Histories { get; set; }        
    }
}
