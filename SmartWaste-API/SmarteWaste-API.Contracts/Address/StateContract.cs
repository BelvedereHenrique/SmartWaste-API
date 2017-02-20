using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Address
{
    public class StateContract
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UF { get; set; }
        public int CountryID { get; set; }
        public CountryContract Country { get; set; }
    }
}
