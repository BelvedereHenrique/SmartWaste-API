using SmarteWaste_API.Contracts;
using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository _repo)
        {
            _countryRepository = _repo;
        }

        public List<CountryContract> GetList()
        {
            return _countryRepository.GetList();
        }
    }
}
