using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class RouteAddModel
    {
        public Guid? RouteID { get; set; }
        public Guid? AssignedToID { get; set; }
        public List<Guid> PointIDs { get; set; }
        public Decimal ExpectedKilometers { get; set; }
        public Decimal ExpectedMinutes { get; set; }
    }
}