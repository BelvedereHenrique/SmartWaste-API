using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business
{
    public class PersonRepository : IPersonRepository
    {
        public PersonContract GetPersonByUserID(Guid userID)
        {
            //TODO: Fix this method to use .ToContract and use filter
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var result = new PersonContract();
                var person = context.Person.FirstOrDefault(x => x.UserID == userID);
                if (person != null)
                {
                    result.UserID = person.UserID;
                    result.Name = person.Name;
                    result.Email = person.Email;
                    result.CompanyID = person.CompanyID;
                }
                return result;
            }
        }
    }
}
