using SmartWaste_API.Services.Interfaces;
using System;
using SmarteWaste_API.Contracts.Account;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business;
using System.Net.Mail;
using SmartWaste_API.Services.Security;

namespace SmartWaste_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IPersonService _personService;
        private readonly IPersonRepository _personRepository;

        public AccountService(IAccountRepository _accRepo, IPersonService _pService, ISecurityManager<IdentityContract> user, IPersonRepository _person)
        {
            _accountRepository = _accRepo;
            _personService = _pService;
            _user = user;
            _personRepository = _person;
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
             
        public AccountService(PersonRepository person, AccountRepository account)
        {
            _personRepository = person;
            _accountRepository = account;
        }

        public void AddPersonal(PersonalSubscriptionFormContract data)
        {
            try
            {
                data.RoleID = Guid.Parse(RolesID.USER_ID);
                _accountRepository.AddPersonal(data);
            }
            catch (Exception e)
            {
                throw new ArgumentException("There was an error while saving the profile: " + e.Message);
            }
        }

        public bool CheckCPFAvailability(string cpf)
        {
            var userByDocument = _personRepository.Get(new PersonFilterContract()
            {
                Document = cpf
            });
            if (userByDocument != null)
                return false;

            return true;
        }

        public bool CheckEmailAvailability(string email)
        {
            try
            {
                var userByEmail = _personRepository.Get(new PersonFilterContract()
                {
                    Email = email
                });

                if (userByEmail != null)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException("There was an error while consulting email availability: " + e.Message);
            }
        }

        public bool ValidatePersonalForm(PersonalSubscriptionFormContract data)
        {
            var mailAddres = new MailAddress(data.Fields.Email);
            if (
                (data.IsValid) &&
                (!String.IsNullOrEmpty(data.Fields.Name.Trim())) &&
                (data.Fields.Password.Length >= 8) &&
                (data.Fields.PasswordConfirmation.Value.Length >= 8) &&
                (data.Fields.Password == data.Fields.PasswordConfirmation.Value) &&
                (data.Fields.CPF.Length == 11) &&
                (!String.IsNullOrEmpty(data.Fields.Name)) &&
                (!String.IsNullOrEmpty(data.Fields.Line1)) &&
                (!String.IsNullOrEmpty(data.Fields.Neighborhood)) &&
                (!String.IsNullOrEmpty(data.Fields.ZipCode))
                )
                return true;

            return false;
        }
    }
}
