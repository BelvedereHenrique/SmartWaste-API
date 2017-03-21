
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
using SmarteWaste_API.Library.Email;
using SmarteWaste_API.Contracts.Password;

namespace SmartWaste_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IParameterService _parameterService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly Library.Email.IEmailSenderService _emailService;

        public UserService(IUserRepository userRepository, IParameterService parameterService, IEmailTemplateService emailTemplateService, Library.Email.IEmailSenderService emailService)
        {
            _userRepository = userRepository;
            _parameterService = parameterService;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
        }

        public UserContract Get(UserFilterContract filter)
        {
            return _userRepository.Get(filter);
        }
        public void SetUserRoles(Guid userID, List<Guid> rolesID)
        {
             _userRepository.SetUserRoles(userID, rolesID);
        }
        public bool CheckUserToken(string email)
        {
            var user = _userRepository.Get(new UserFilterContract() { Login = email });
            if (user == null) throw new ArgumentException("There is no user with this email");
            if (user.RecoveryToken != null && user.RecoveredOn == null)
                if (user.ExpirationDate >= DateTime.Now) return true;
            return false;
        }
        public async Task SendToken(string email)
        {

            var token = _userRepository.SaveToken(email);
            var model = new EmailContract()
            {
                Email = email,
                Informations = _parameterService.GetEmailSenderInformations(),
            };

            var emailTemplate = _emailTemplateService.GetEmailTemplate("sendtoken");
            var emailSubject = "SmartWaste: No-Reply";
            var message = emailTemplate;
            message = message.Replace("{Token}",token);
            message = message.Replace("{Email}", email);
            await _emailService.SendEmailAsync(model.Informations, model.Email, emailSubject, message, null);
        }

        public void ChangePassword(PasswordContract password)
        {
            var user = _userRepository.Get(new UserFilterContract() { Login = password.Email });
            if (user == null) throw new ArgumentException("There is no user with this email");
            if (user.ExpirationDate < DateTime.Now) throw new ArgumentException("The token is expired");
            if (user.RecoveryToken != password.Token) throw new ArgumentException("The token is incorrect");
            if (user.RecoveredOn != null) throw new ArgumentException("The token has already been used");
            _userRepository.ChangePassword(user.ID ,password);
        }

    }
}
