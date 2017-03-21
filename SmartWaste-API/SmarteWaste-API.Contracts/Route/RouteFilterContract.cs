using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Route
{
    public class RouteFilterContract
    {
        public Guid? ID { get; set; }
        public Guid? AssignedToID { get; set; }
        public Guid? CreatedBy { get; set; }        
        public Guid? CompanyID { get; set; }
        public RouteStatusEnum? Status { get; set; }
        public RouteStatusEnum? NotStatus { get; set; }
    }
}
