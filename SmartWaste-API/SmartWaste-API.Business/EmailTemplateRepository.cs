using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        public string GetEmailTemplate(string name)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return context.EmailTemplates.First(x => x.Name.ToLower().Contains(name)).Email;
            }
        }
    }
}
