using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Address
{
    public class CityContract
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public StateContract State { get; set; }
    }
}
