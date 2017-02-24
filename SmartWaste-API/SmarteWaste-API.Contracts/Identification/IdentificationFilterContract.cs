using System;
using System.Runtime.Serialization;

namespace SmarteWaste_API.Contracts.Identification
{
    [DataContract]
    public class IdentificationFilterContract
    {
        [DataMember]
        public int? ID { get; set; }

        [DataMember]
        public int? Value { get; set; }

        [DataMember]
        public int? IdentificationTypeID { get; set; }

        [DataMember]
        public Guid? PersonID { get; set; }

    }
}
