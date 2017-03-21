using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RouteContract
    {
        public Guid ID { get; set; }
        public Person.PersonContract AssignedTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public RouteStatusEnum Status { get; set; }
        public Person.PersonContract CreatedBy { get; set; }
        public Guid CompanyID { get; set; }
        public Decimal ExpectedKilometers { get; set; }
        public Decimal ExpectedMinutes { get; set; }
    }
}
