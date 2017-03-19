using SmarteWaste_API.Contracts.Password;
using SmarteWaste_API.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IUserRepository
    {
        UserContract Get(UserFilterContract filter);
        List<UserContract> GetList(UserFilterContract filter);
        void SetUserRoles(Guid userID, List<Guid> rolesID);
        string SaveToken(string email);
        void ChangePassword(Guid ID, PasswordContract password);
    }
}
