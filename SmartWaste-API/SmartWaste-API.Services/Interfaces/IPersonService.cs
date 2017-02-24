using SmarteWaste_API.Contracts.Person;
using System;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IPersonService
    {
        PersonContract Get(PersonFilterContract filter);
        void SetCompanyID(Guid companyID, PersonFilterContract filter);
    }
}
