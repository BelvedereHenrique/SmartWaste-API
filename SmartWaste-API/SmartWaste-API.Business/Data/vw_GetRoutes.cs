//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartWaste_API.Business.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_GetRoutes
    {
        public System.Guid ID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ClosedOn { get; set; }
        public int StatusID { get; set; }
        public decimal ExpectedKilometers { get; set; }
        public decimal ExpectedMinutes { get; set; }
        public Nullable<System.Guid> AssignedToID { get; set; }
        public System.Guid CompanyID { get; set; }
        public Nullable<System.DateTime> NavigationStartedOn { get; set; }
        public Nullable<System.DateTime> NavigationFinishedOn { get; set; }
        public string AssignedToName { get; set; }
        public System.Guid CreatedByID { get; set; }
        public string CreatedByName { get; set; }
    }
}
