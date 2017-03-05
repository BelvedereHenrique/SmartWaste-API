using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Security;

namespace SmartWaste_API.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ISecurityManager<IdentityContract> _user;

        public PersonService(IPersonRepository _repo, ISecurityManager<IdentityContract> user)
        {
            _personRepository = _repo;
            _user = user;
        }

        public PersonContract Get(PersonFilterContract filter)
        {
            return _personRepository.Get(filter);
        }

        public List<PersonContract> GetList(PersonFilterContract filter)
        {
            Authorize();

            return _personRepository.GetList(filter);
        }

        public void SetCompanyID(Guid companyID, PersonFilterContract filter)
        {
            _personRepository.SetCompanyID(companyID, filter);
        }

        private void Authorize()
        {
            if (!_user.User.IsAuthenticated || !_user.IsInRole(RolesName.COMPANY_USER))
                throw new UnauthorizedAccessException("You are not authorized.");
        }
    }
}
