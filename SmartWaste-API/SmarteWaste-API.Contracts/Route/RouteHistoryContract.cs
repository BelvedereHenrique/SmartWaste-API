using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RouteHistoryContract
    {
        public RouteHistoryContract() {
            Person = new Contracts.Person.PersonContract();
        }

        public Guid ID { get; set; }
        public Guid RouteID { get; set; }
        public RouteStatusEnum Status { get; set; }
        public Person.PersonContract Person { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
    }
}
