using SmarteWaste_API.Contracts.Address;
using System;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IAddressRepository
    {
        AddressContract GetAddress(Guid addressID);
        void Add(AddressContract contract);
    }
}
