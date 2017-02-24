using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointFilterContract
    {
        public PointCoordinatorContract Northwest { get; set; }
        public PointCoordinatorContract Southeast { get; set; }
    }
}
