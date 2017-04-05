using System.Runtime.Serialization;

namespace SmarteWaste_API.Contracts.Person
{
    [DataContract]
    public class PersonalSubscriptionFormDataContract
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public PersonalSubscriptionPasswordContract PasswordConfirmation { get; set; }
        [DataMember]
        public string CPF { get; set; }
        [DataMember]
        public int Country { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int City { get; set; }
        [DataMember]
        public string Line1 { get; set; }
        [DataMember]
        public string Line2 { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Neighborhood { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
    }
}
