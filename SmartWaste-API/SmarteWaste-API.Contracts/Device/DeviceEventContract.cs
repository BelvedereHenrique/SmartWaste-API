using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Device
{
    public class DeviceEventContract
    {
        public string SerialNumber { get; set; }
        public string BatteryVoltage { get; set; }
        public string ClickType { get; set; }
    }
}
