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
using System.Collections.Generic;
using System.Threading.Tasks;
using SmarteWaste_API.Library.Email;
using SmartWaste_API.Library.Email;
using System.Globalization;

namespace SmartWaste_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        private readonly IParameterService _parameterService;
        private readonly IEmailSenderService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public AccountService (IAccountRepository _accRepo, IPersonService _pService, IUserService _uService, IParameterService _parameter,IEmailSenderService _email ,IEmailTemplateService _etemplate, ISecurityManager<IdentityContract> user)
        {
            _accountRepository = _accRepo;
            _personService = _pService;
            _userService = _uService;
            _emailService = _email;
            _parameterService = _parameter;
            _emailTemplateService = _etemplate;
            _user = user;
        }

        /// <summary>
        /// Add an enterprise, set the enterpriseID into Logged User and Restart his password
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns>EnterpriseID</returns>
        public async Task<Guid> DoChangesToNewEnterprise (AccountEnterpriseContract enterprise)
        {
            try
            {
                if (!IsAuthenticatedUser()) throw new UnauthorizedAccessException(Resources.MessagesResources.AccountServiceMessages.USER_NOT_AUTHENTICATED);
                var e = AddEnterprise(enterprise);
                AddEnterpriseToLoggedUser(e);
                SetCompanyRolesToLoggedUser();
                RestartPassword();
                await SendEnterpriseEmail(enterprise.Name);
                return e;
            }
            catch (Exception ex){ throw ex; }
        }

        /// <summary>
        /// Add an enterprise into database
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns>EnterpriseID</returns>
        public Guid AddEnterprise (AccountEnterpriseContract enterprise)
        {
            if (CheckEnterprise (enterprise))
                throw new ArgumentException (Resources.MessagesResources.AccountServiceMessages.ENTERPRISE_ALREADY_REGISTRED);
            if (GetUserEnterprise().ID.HasValue)
                throw new ArgumentException(Resources.MessagesResources.AccountServiceMessages.USER_ALREADY_ASSOCIATED_ENTERPRISE);
            return _accountRepository.AddEnterprise (enterprise);
        }

        /// <summary>
        /// Set enterpriseID to Logged User
        /// </summary>
        /// <param name="enterpriseID"></param>
        private void AddEnterpriseToLoggedUser (Guid enterpriseID)
        {
            _personService.SetCompanyID (enterpriseID, new SmarteWaste_API.Contracts.Person.PersonFilterContract() { UserID = _user.User.User.ID });
        }

        private void SetCompanyRolesToLoggedUser()
        {
            var listRoles = new List<Guid>();
            listRoles.Add(Guid.Parse(RolesID.COMPANY_ADMIN_ID));
            listRoles.Add(Guid.Parse(RolesID.COMPANY_ROUTE_ID));
            listRoles.Add(Guid.Parse(RolesID.COMPANY_USER_ID));
            _userService.SetUserRoles(_user.User.User.ID, listRoles);
        }

        /// <summary>
        /// Restart loggedUser password
        /// </summary>
        private void RestartPassword () { }

        /// <summary>
        /// Verify if company is already registred
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public bool CheckEnterprise (AccountEnterpriseContract enterprise)
        {
            return _accountRepository.CheckEnterprise(enterprise);
        }

        /// <summary>
        /// Get Enterprise associated with logged User Account
        /// </summary>
        /// <returns></returns>
        public AccountEnterpriseContract GetUserEnterprise()
        {
            if (!IsAuthenticatedUser()) throw new UnauthorizedAccessException(Resources.MessagesResources.AccountServiceMessages.USER_NOT_AUTHENTICATED);
            return _accountRepository.GetUserEnterprise(_user.User.User.ID);
        }

        public async Task SendEnterpriseEmail(string enterpriseName)
        {
            var model = new EmailContract()
            {
                Email = _user.User.Login,
                //Email = "felipebotelhofelipe@hotmail.com",
                Informations = _parameterService.GetEmailSenderInformations(),
                UserName = _user.User.Person.Name
            };

            var emailTemplate = _emailTemplateService.GetEmailTemplate("createenterprise");
            var emailSubject = "SmartWaste: No-Reply";
            var message = emailTemplate;
            message = message.Replace("{UserName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.UserName));
            message = message.Replace("{EnterpriseName}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enterpriseName));
            await _emailService.SendEmailAsync(model.Informations, model.Email, emailSubject, message, null);
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
            var userByDocument = _personService.Get(new PersonFilterContract()
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
                var userByEmail = _personService.Get(new PersonFilterContract()
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
        private bool IsAuthenticatedUser()
        {
            return _user.User.IsAuthenticated;
        }
    }
}
