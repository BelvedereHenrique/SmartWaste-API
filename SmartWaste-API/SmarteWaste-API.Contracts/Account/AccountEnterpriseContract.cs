using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Account
{
    public class AccountEnterpriseContract
    {
        public string  Name { get; set; }
        public string CNPJ { get; set; }
        public Address.AddressContract Address { get; set; }
    }
}
