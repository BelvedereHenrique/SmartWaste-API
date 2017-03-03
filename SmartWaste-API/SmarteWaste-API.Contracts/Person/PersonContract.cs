using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Person
{
    public class PersonContract
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public Guid? CompanyID { get; set; }
        public Contracts.Account.AccountEnterpriseContract Company { get; set; }
        public PersonTypeEnum PersonType { get; set; }
        public string Email { get; set; }
    }
}
