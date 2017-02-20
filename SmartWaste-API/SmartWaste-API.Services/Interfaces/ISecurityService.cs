using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface ISecurityService
    {
        OperationResult<IdentityContract> SignIn(string userName, string password);
    }
}
