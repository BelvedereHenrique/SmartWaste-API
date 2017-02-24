using SmarteWaste_API.Contracts.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointContract
    {
        public Guid ID { get; set; }
        public PointStatusEnum Status { get; set; }
        public PointTypeEnum Type { get; set; }
        public Nullable<System.Guid> DeviceID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public System.Guid PersonID { get; set; }
        public System.Guid UserID { get; set; }
    }
}
