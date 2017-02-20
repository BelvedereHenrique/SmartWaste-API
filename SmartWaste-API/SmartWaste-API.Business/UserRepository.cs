using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Business.ContractParser;

namespace SmartWaste_API.Business
{
    public class UserRepository : IUserRepository
    {
        public UserContract Get(UserFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return Filter(context, filter).FirstOrDefault().ToContract();
            }
        }

        public List<UserContract> GetList(UserFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return Filter(context, filter).ToList().ToContracts();
            }
        }

        public IQueryable<Data.User> Filter(Data.SmartWasteDatabaseConnection context, UserFilterContract filter)
        {
            return context.User.Where(user => 
                (filter.ID == null || filter.ID == user.ID) &&
                (filter.Login == String.Empty || filter.Login.ToLower().Trim() == user.Login.ToLower().Trim())
            ).AsQueryable();
        }
    }
}
