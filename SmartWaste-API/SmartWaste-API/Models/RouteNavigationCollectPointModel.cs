using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class RouteNavigationCollectPointModel : RouteNavigationModel
    {
        public Guid PointID { get; set; }
        public bool Collected { get; set; }
        public string Reason { get; set; }
    }
}