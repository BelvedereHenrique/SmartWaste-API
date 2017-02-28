using SmarteWaste_API.Contracts.Identification;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IIdentificationRepository
    {
        IdentificationContract Get(IdentificationFilterContract filter);
    }
}
