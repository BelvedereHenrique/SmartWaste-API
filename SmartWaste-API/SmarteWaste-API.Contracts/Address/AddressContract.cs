using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Address
{
    public class AddressContract
    {
        public Guid ID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public CityContract City { get; set; }
    }
}
