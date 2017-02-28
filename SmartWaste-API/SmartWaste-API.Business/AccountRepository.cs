using SmartWaste_API.Business.Interfaces;
using System;
using SmarteWaste_API.Contracts.Account;
using System.Linq;

namespace SmartWaste_API.Business
{
    public class AccountRepository : IAccountRepository
    {

        public bool CheckEnterprise(AccountEnterpriseContract enterprise)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var e = context.Companies.FirstOrDefault(x => x.Name.ToLower().Equals(enterprise.Name.ToLower()) || x.CNPJ == enterprise.CNPJ);
                return e != null;
            }

        }

        public Guid AddEnterprise(AccountEnterpriseContract enterprise)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var enterpriseID = Guid.NewGuid();
                        var addressID = Guid.NewGuid();
                        context.Companies.Add(new Data.Company()
                        {
                            ID = enterpriseID,
                            CNPJ = enterprise.CNPJ,
                            Name = enterprise.Name
                        });
                        context.Addresses.Add(new Data.Address()
                        {
                            ID = addressID,
                            CityID = enterprise.Address.City.ID,
                            Line1 = enterprise.Address.Line1,
                            ZipCode = enterprise.Address.ZipCode,
                            Neighborhood = enterprise.Address.Neighborhood,
                            Latitude = enterprise.Address.Latitude,
                            Longitude = enterprise.Address.Longitude
                        });
                        context.CompanyAddresses.Add(new Data.CompanyAddress()
                        {
                            ID = Guid.NewGuid(),
                            AddressID = addressID,
                            CompanyID = enterpriseID
                        });                   
                        context.SaveChanges();
                        transaction.Commit();
                        return enterpriseID;
                    }
                    catch (Exception ex)
                    {   
                        transaction.Rollback();
                        throw;
                    }
                }

            }
         }
      }
}
