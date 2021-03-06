﻿using SmarteWaste_API.Contracts.Person;
using SmartWaste_API.Business.ContractParser;
using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartWaste_API.Business
{
    public class PersonRepository : IPersonRepository
    {
        public PersonContract Get(PersonFilterContract filter)
        {            
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return Filter(context, filter).FirstOrDefault().ToContract();                
            }
        }

        public List<PersonContract> GetList(PersonFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return Filter(context, filter).ToList().ToContracts();
            }
        }
        public void SetCompanyID(Guid companyID, PersonFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var p = Filter(context,filter).FirstOrDefault();
                p.CompanyID = companyID;
                context.SaveChanges();
            }
        }

        private IQueryable<Data.Person> Filter(Data.SmartWasteDatabaseConnection context, PersonFilterContract filter)
        {
            return context.People.Where(x =>
                (filter.ID == null || filter.ID == x.ID) &&
                (filter.UserID == null || filter.UserID == x.UserID) &&
                (filter.CompanyID == null || filter.CompanyID == x.CompanyID)&&
                (String.IsNullOrEmpty(filter.Email) || filter.Email == x.Email)&&
                (String.IsNullOrEmpty(filter.Document) || x.Identifications.Any(y=>y.Value == filter.Document))
            ).OrderBy(x => x.Name);
        }
    }
}
