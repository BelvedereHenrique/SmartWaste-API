using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Account
{
    public class EmployeeCompanyRequestContract
    {
        public Guid? ID { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedON { get; set; }
        public DateTime? ClosedON { get; set; }
        public Person.PersonContract CreatedBy { get; set; }
        public Guid PersonID { get; set; }
        public string Email { get; set; }
        public Guid CompanyID { get; set; }
    }
}
