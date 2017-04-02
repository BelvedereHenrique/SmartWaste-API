using System;
using System.Runtime.Serialization;

namespace SmarteWaste_API.Contracts.Device
{
    [DataContract]
    public class DeviceContract
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public DeviceTypeEnum Type { get; set; }

        [DataMember]
        public DeviceStatusEnum Status { get; set; }

        [DataMember]
        public string InternalID { get; set; }

        [DataMember]
        public int? BatteryVoltage { get; set; }
    }
}