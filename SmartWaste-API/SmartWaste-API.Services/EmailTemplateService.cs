using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailTemplateService(IEmailTemplateRepository _repo)
        {
            _emailTemplateRepository = _repo;
        }
        public string GetEmailTemplate(string name)
        {
            return _emailTemplateRepository.GetEmailTemplate(name);
        }
    }
}
