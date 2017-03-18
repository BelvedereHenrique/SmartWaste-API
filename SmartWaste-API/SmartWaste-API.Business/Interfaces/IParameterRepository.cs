using SmarteWaste_API.Library.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IParameterRepository
    {
        SenderInformationsContract GetEmailSenderInformations();
    }
}
