
using SmarteWaste_API.Library.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace SmartWaste_API.Library.Email
{
    public class EmailSenderService: IEmailSenderService
    {
        public async  Task SendEmailAsync(SenderInformationsContract informations, string email, string subject, string message, List<HttpPostedFileBase> attachments)
        {
            try
            {
                var _email = informations.Email;
                var _epass = Library.SimetrictEncriptHelper.Decrypt(informations.Password,true);
                var _dispName = "SmartWaste";
                MailMessage myMessage = new MailMessage();
                if (attachments != null && attachments.Count > 0 && attachments[0] != null)
                {
                    foreach (var attachment in attachments)
                    {
                        var fileName = Path.GetFileName(attachment.FileName);
                        myMessage.Attachments.Add(new Attachment(attachment.InputStream, fileName));
                    }
                }
                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _dispName);
                myMessage.Subject = subject;
                myMessage.Body = message;
                myMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = informations.SMTP; //if using hotmail to send emails 
                    //smtp.Host = "smtp.live.com"; //if using hotmail to send emails 
                    smtp.Port = int.Parse(informations.Port);
                    //smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _epass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
