using SmarteWaste_API.Library.Email;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class ParameterService: IParameterService
    {
        private readonly IParameterRepository _parameterRepository;

        public ParameterService(IParameterRepository _repo)
        {
            _parameterRepository = _repo;
        }

        public SenderInformationsContract GetEmailSenderInformations()
        {
            return _parameterRepository.GetEmailSenderInformations();
        }
    }
}
