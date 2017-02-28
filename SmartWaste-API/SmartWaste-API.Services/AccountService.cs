using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Account;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;

namespace SmartWaste_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IPersonService _personService;

        public AccountService(IAccountRepository _accRepo, IPersonService _pService, ISecurityManager<IdentityContract> user)
        {
            _accountRepository = _accRepo;
            _personService = _pService;
            _user = user;
        }


        public Guid AddEnterprise(AccountEnterpriseContract enterprise)
        {
            if (!CheckEnterprise(enterprise))
            {
                var entID = _accountRepository.AddEnterprise(enterprise);
                _personService.SetCompanyID(entID, new SmarteWaste_API.Contracts.Person.PersonFilterContract() { UserID = _user.User.User.ID });
               //SetCompanyID to Person
               //Restart the password.
                return entID;

            }
            else
                throw new ArgumentException("Enterprise already Registered");
        }
        public bool CheckEnterprise(AccountEnterpriseContract enterprise)
        {
            return _accountRepository.CheckEnterprise(enterprise);
        }
             
    }
}
