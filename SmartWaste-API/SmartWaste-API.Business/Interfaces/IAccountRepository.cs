using SmarteWaste_API.Contracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IAccountRepository
    {
        Guid AddEnterprise(AccountEnterpriseContract enterprise);
        bool CheckEnterprise(AccountEnterpriseContract enterprise);
    }
}
