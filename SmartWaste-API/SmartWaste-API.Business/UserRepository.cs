using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Business.ContractParser;
using SmarteWaste_API.Contracts.Password;
using SmartWaste_API.Library;

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

        public string SaveToken(string email)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var user = Filter(context,new UserFilterContract() { Login = email }).FirstOrDefault();
                if (user == null) throw new ArgumentException("There is no user with this email");
                var token = Guid.NewGuid().ToString().Substring(0,10);
                user.RecoveryToken = token.ToUpper();
                user.ExpirationDate = DateTime.Now.AddDays(1);
                user.RecoveredOn = null;
                context.SaveChanges();
                return token;
            }
        }

        public void ChangePassword(Guid ID, PasswordContract password)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var user = context.Users.First(x => x.ID == ID);
                user.Password = MD5Helper.Create(password.Password);
                user.RecoveredOn = DateTime.Now;
                context.SaveChanges();
            }
        }

        public IQueryable<Data.User> Filter(Data.SmartWasteDatabaseConnection context, UserFilterContract filter)
        {
            return context.Users.Where(user => 
                (filter.ID == null || filter.ID == user.ID) &&
                (String.IsNullOrEmpty(filter.Login) || filter.Login.ToLower().Trim() == user.Login.ToLower().Trim())
            ).AsQueryable();
        }

        public void SetUserRoles(Guid userID, List<Guid> rolesID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                foreach (var role in rolesID)
                {
                    if(context.UserRoles.FirstOrDefault(x=>x.UserID == userID && x.RoleID == role) == null)
                        context.UserRoles.Add(new Data.UserRole() { ID = Guid.NewGuid(), RoleID = role, UserID = userID });
                }
                context.SaveChanges();
            }
        }
    }
}
