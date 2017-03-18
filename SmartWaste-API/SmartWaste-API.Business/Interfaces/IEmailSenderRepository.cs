using SmarteWaste_API.Library.Email;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IEmailSenderRepository
    {
        SenderInformationsContract GetSenderInformations();
    }
}
