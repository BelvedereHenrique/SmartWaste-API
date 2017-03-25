using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Person;
using System;
using System.Threading.Tasks;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Guid> DoChangesToNewEnterprise(AccountEnterpriseContract enterprise);
        Guid AddEnterprise(AccountEnterpriseContract enterprise);
        AccountEnterpriseContract GetUserEnterprise();
        bool CheckEnterprise(AccountEnterpriseContract enterprise);
        void AddPersonal(PersonalSubscriptionFormContract data);
        bool CheckCPFAvailability(string cpf);
        bool CheckEmailAvailability(string email);
        bool ValidatePersonalForm(PersonalSubscriptionFormContract data);
        Task SendEmployeeEnterpriseTokenEmail(string email, bool verify);
        void SetEnterprisePermission(string email, string password, string token);
    }
}
