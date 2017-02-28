using SmarteWaste_API.Contracts.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IPersonRepository
    {
        PersonContract Get(PersonFilterContract filter);
        List<PersonContract> GetList(PersonFilterContract filter);
        void SetCompanyID(Guid companyID, PersonFilterContract filter);
    }
}
