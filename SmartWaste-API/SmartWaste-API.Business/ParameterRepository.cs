using SmarteWaste_API.Library.Email;
using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business
{
    public class ParameterRepository: IParameterRepository
    {
        public SenderInformationsContract GetEmailSenderInformations()
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var result = new SenderInformationsContract();
                result.Email = context.Parameters.First(x => x.Name.ToLower().Contains("email_smartwaste")).Value;
                result.Password = context.Parameters.First(x => x.Name.ToLower().Contains("email_password_smartwaste")).Value;
                result.Port = context.Parameters.First(x => x.Name.ToLower().Contains("email_port_smartwaste")).Value;
                result.SMTP = context.Parameters.First(x => x.Name.ToLower().Contains("email_smtp_smartwaste")).Value;
                return result;
            }

        }
    }
}
