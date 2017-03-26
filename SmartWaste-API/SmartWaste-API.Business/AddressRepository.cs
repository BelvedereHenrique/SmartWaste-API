using SmartWaste_API.Business.Interfaces;
using System;
using System.Linq;
using SmarteWaste_API.Contracts.Address;
using SmartWaste_API.Business.Data;
using SmartWaste_API.Business.ContractParser;

namespace SmartWaste_API.Business
{
    public class AddressRepository : IAddressRepository
    {
        public void Add(AddressContract contract)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var address = new Address()
                {
                   ID = contract.ID,
                   Line1 = contract.Line1,
                   Line2 = contract.Line2,
                   ZipCode = contract.ZipCode,
                   Neighborhood = contract.Neighborhood,
                   Latitude = contract.Latitude,
                   Longitude = contract.Longitude,
                   CityID = contract.City.ID
                };
                context.Addresses.Add(address);
                context.SaveChanges();
            }
        }

        public AddressContract GetAddress(Guid addressID)
        {
            using (var context = new SmartWasteDatabaseConnection())
            {
                var contract = context.Addresses.SingleOrDefault(x=>x.ID == addressID);
                return new AddressContract() {
                    ID = contract.ID,
                    City = context.Cities.SingleOrDefault(x => x.ID == contract.City.ID).ToContract(),
                    Latitude = contract.Latitude,
                    Longitude = contract.Longitude,
                    Line1 = contract.Line1,
                    Line2 = contract.Line2,
                    Neighborhood = contract.Neighborhood,
                    ZipCode = contract.ZipCode
                };
            }
        }
    }
}
