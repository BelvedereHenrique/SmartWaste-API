using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Person;
using System;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IAccountService
    {
        Guid AddEnterprise(AccountEnterpriseContract enterprise);
        void AddPersonal(PersonalSubscriptionFormContract data);
        bool CheckCPFAvailability(string cpf);
        bool CheckEmailAvailability(string email);
        bool ValidatePersonalForm(PersonalSubscriptionFormContract data);
    }
}
