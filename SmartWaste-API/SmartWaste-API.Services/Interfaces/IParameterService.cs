using SmarteWaste_API.Library.Email;

namespace SmartWaste_API.Services.Interfaces
{
    public interface IParameterService
    {
        SenderInformationsContract GetEmailSenderInformations();
    }
}
