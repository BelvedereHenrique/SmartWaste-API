﻿using SmartWaste_API.Business.Interfaces;
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
