using SmartWaste_API.Business.Interfaces;
using System;
using SmarteWaste_API.Contracts.Account;
using System.Linq;
using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Library;

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
                            Line2 = enterprise.Address.Line2,
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
                    
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public void AddPersonal(PersonalSubscriptionFormContract data)
        {
            var user = new Data.User()
            {
                ID = Guid.NewGuid(),
                Login = data.Fields.Email,
                Password = MD5Helper.Create(data.Fields.Password)
            };

            var userRole = new Data.UserRole()
            {
                ID = Guid.NewGuid(),
                UserID = user.ID,
                RoleID = data.RoleID
            };
            var personContract = new Data.Person()
            {
                ID = Guid.NewGuid(),
                CompanyID = null,
                Email = data.Fields.Email,
                PersonTypeID = 1,
                Name = data.Fields.Name,
                UserID = user.ID
            };
            var identification = new Data.Identification()
            {
                ID = Guid.NewGuid(),
                IdentificationTypeID = 1,
                PersonID = personContract.ID,
                Value = data.Fields.CPF
            };
            var address = new Data.Address()
            {
                ID = Guid.NewGuid(),
                CityID = data.Fields.City,
                Line1 = data.Fields.Line1,
                Line2 = data.Fields.Line2,
                Neighborhood = "",
                ZipCode = data.Fields.ZipCode,
            };
            var personAddress = new Data.PersonAddress()
            {
                ID = Guid.NewGuid(),
                PersonID = personContract.ID,
                AddressID = address.ID
            };
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Users.Add(user);
                        context.UserRoles.Add(userRole);
                        context.People.Add(personContract);
                        context.Identifications.Add(identification);
                        context.Addresses.Add(address);
                        context.PersonAddresses.Add(personAddress);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }

        public AccountEnterpriseContract GetUserEnterprise(Guid userID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var enterpriseReturn = new AccountEnterpriseContract();
                var person = context.People.First(x => x.UserID == userID);
                if (person.CompanyID.HasValue)
                {
                    enterpriseReturn.Name = person.Company.Name;
                    enterpriseReturn.CNPJ = person.Company.CNPJ;
                    enterpriseReturn.ID = person.Company.ID;
                }
                return enterpriseReturn;
            }
        }

        public EmployeeCompanyRequestContract GetEmployeeRequest(string email)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var result = new EmployeeCompanyRequestContract();
                var request = context.EmployeeCompanyRequests.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()) && x.ClosedOn == null);
                if(request != null)
                {
                    result.ID = request.ID;
                    result.CreatedON = request.CreatedOn;
                    result.CreatedBy = new PersonContract()
                    {
                        ID = request.Person.ID,
                        Name = request.Person.Name,
                        Email = request.Person.Email
                    };
                    result.Email = request.Email;
                    result.Token = request.Link;
                    result.CompanyID = request.CompanyID;
                    result.PersonID = request.PersonID.Value;
                }
                return result;
            }
        }
        public string SaveEnterpriseToken(string email, Guid sender)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                CloseOpenedEnterpriseToken(email);
                var user = context.People.First(x=>x.Email == email);
                var token = Guid.NewGuid().ToString().Substring(0, 10);
                context.EmployeeCompanyRequests.Add(new Data.EmployeeCompanyRequest()
                {
                    ID = Guid.NewGuid(),
                    ClosedOn = null,
                    CreatedByID = sender,
                    CreatedOn = DateTime.Now,
                    Email = email,
                    Link = token,
                    PersonID = user.ID,
                    CompanyID = context.People.Where(x => x.ID == sender).First().CompanyID.Value
                });
                context.SaveChanges();
                return token;
            }   
        }

        public void CloseOpenedEnterpriseToken(string email)
        {
            using (var c = new Data.SmartWasteDatabaseConnection())
            {
                var request = c.EmployeeCompanyRequests.FirstOrDefault(x => x.Email == email && x.ClosedOn == null);
                if (request != null)
                {
                    request.ClosedOn = DateTime.Now;
                    c.SaveChanges();
                }
            }
        }
        public void SetCompanyID(Guid personID ,Guid companyID)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var person = context.People.First(x => x.ID == personID);
                person.CompanyID = companyID;
                context.SaveChanges();
            }
        }

    }
}
