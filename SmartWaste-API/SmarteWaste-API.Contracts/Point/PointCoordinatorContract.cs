using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointCoordinatorContract
    {
        public Decimal? Latitude { get; set; }
        public Decimal? Longitude { get; set; }
    }
}
