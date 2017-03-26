using System;
using System.Runtime.Serialization;

namespace SmarteWaste_API.Contracts.Device
{
    [DataContract]
    public class DeviceHistoryContract
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid DeviceID { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public DeviceStatusEnum Status { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public Guid? PersonID { get; set; }

    }
}