using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Person
{
    [DataContract]
    public class PersonalSubscriptionPasswordContract
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
    }
}
