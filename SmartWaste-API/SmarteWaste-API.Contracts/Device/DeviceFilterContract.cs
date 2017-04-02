using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Device
{
    public class DeviceFilterContract
    {
        public Guid? ID { get; set; }        
        public string InternalID { get; set; }
        public DeviceStatusEnum? Status { get; set; }
        public DeviceTypeEnum? Type { get; set; }
    }
}
