using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Person;
using System;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IAccountService
    {
        Guid DoChangesToNewEnterprise(AccountEnterpriseContract enterprise);
        Guid AddEnterprise(AccountEnterpriseContract enterprise);
        AccountEnterpriseContract GetUserEnterprise();
        bool CheckEnterprise(AccountEnterpriseContract enterprise);
        void AddPersonal(PersonalSubscriptionFormContract data);
        bool CheckCPFAvailability(string cpf);
        bool CheckEmailAvailability(string email);
        bool ValidatePersonalForm(PersonalSubscriptionFormContract data);
    }
}
