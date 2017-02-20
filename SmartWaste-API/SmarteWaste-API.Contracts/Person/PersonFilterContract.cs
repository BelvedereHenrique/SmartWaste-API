using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Person
{
    public class PersonFilterContract
    {
        public Guid? ID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? CompanyID { get; set; }
    }
}
