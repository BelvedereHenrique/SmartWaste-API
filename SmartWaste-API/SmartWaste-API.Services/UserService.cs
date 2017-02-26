
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.User;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library;

namespace SmartWaste_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserContract Get(UserFilterContract filter)
        {
            return _userRepository.Get(filter);
        }
    }
}
