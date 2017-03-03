using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IUserService
    {
        UserContract Get(UserFilterContract filter);
        void SetUserRoles(Guid userID, List<Guid> rolesID);
    }
}
