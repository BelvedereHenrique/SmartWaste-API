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
    
    public partial class vw_GetPoints
    {
        public System.Guid ID { get; set; }
        public int StatusID { get; set; }
        public int PointRouteStatusID { get; set; }
        public Nullable<System.Guid> DeviceID { get; set; }
        public int TypeID { get; set; }
        public System.Guid AddressID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public System.Guid PersonID { get; set; }
        public System.Guid UserID { get; set; }
    }
}
