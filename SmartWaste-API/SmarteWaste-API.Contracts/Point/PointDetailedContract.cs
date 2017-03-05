using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointDetailedContract : PointContract
    {        
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string StateAlias { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryAlias { get; set; }
        public string Name { get; set; }
    }
}
