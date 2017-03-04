using SmarteWaste_API.Contracts.Account;
using SmarteWaste_API.Contracts.Person;
using System;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IAccountRepository
    {
        Guid AddEnterprise(AccountEnterpriseContract enterprise);
        bool CheckEnterprise(AccountEnterpriseContract enterprise);
        void AddPersonal(PersonalSubscriptionFormContract data);
    }
}