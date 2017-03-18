using SmarteWaste_API.Library.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SmartWaste_API.Library.Email
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(SenderInformationsContract informations, string email, string subject, string message, List<HttpPostedFileBase> attachments);
    }
}
