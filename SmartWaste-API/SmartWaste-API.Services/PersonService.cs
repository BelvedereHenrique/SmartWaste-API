using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace SmartWaste_API.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository _repo)
        {
            _personRepository = _repo;
        }

        public PersonContract Get(PersonFilterContract filter)
        {
            return _personRepository.Get(filter);
        }

        public void SetCompanyID(Guid companyID, PersonFilterContract filter)
        {
            _personRepository.SetCompanyID(companyID, filter);
        }
    }
}
