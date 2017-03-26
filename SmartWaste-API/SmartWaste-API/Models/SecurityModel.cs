using Newtonsoft.Json;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class SecurityModel
    {
        public Guid PersonID { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        [JsonIgnore]
        public List<string> Roles { get; set; }
        [JsonIgnore]
        public IdentityContract Identity { get; set; }
        public SecurityModel(IdentityContract identity)
        {
            this.Identity = identity;
            this.PersonID = identity.Person.ID;
            this.Name = identity.Person.Name;
            this.CompanyName = identity.Person.Company != null ? identity.Person.Company.Name : string.Empty;
            this.Roles = identity.Roles;
        }

        public bool ShowRoutesMenu
        {
            get
            {
                return this.CanNavigateRoutes || this.CanSaveRoutes;
            }
        }

        public bool CanSaveRoutes
        {
            get
            {
                return this.Roles.Any(x => x == RolesName.COMPANY_ROUTE);
            }
        }

        public bool CanNavigateRoutes
        {
            get
            {
                return this.Roles.Any(x => x == RolesName.COMPANY_USER);
            }
        }

        public bool CanSetTrashcanAsFull
        {
            get
            {
                return this.Identity.Person.CompanyID == null;
            }
        }

        public bool CanSeeAllPointDetails
        {
            get
            {
                return this.Roles.Any(x => x == RolesName.COMPANY_USER);
            }
        }

        public bool CanSeeMapLegendColors
        {
            get
            {
                return this.Roles.Any(x => x == RolesName.COMPANY_USER);
            }
        }
    }
}