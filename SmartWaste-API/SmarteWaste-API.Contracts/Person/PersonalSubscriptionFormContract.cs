using System;
using System.Runtime.Serialization;

namespace SmarteWaste_API.Contracts.Person
{
    [DataContract]
    public class PersonalSubscriptionFormContract
    {
        [DataMember]
        public PersonalSubscriptionFormDataContract Fields { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public Guid RoleID{ get; set; }
    }
}