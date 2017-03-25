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
        AccountEnterpriseContract GetUserEnterprise(Guid userID);
        EmployeeCompanyRequestContract GetEmployeeRequest(Guid employeeID);
        string SaveEnterpriseToken(string email, Guid sender);
    }
}
