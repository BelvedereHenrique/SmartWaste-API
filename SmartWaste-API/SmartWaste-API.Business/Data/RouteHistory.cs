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
    
    public partial class RouteHistory
    {
        public System.Guid ID { get; set; }
        public System.Guid RouteID { get; set; }
        public int StatusID { get; set; }
        public System.Guid PersonID { get; set; }
        public string Reason { get; set; }
    
        public virtual Route Route { get; set; }
        public virtual RouteStatu RouteStatu { get; set; }
    }
}
