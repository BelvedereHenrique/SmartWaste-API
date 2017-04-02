using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class DeviceEventModel
    {
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
        [JsonProperty("batteryVoltage")]
        public string BatteryVoltage { get; set; }
        [JsonProperty("clickType")]
        public string ClickType { get; set; }
    }
}