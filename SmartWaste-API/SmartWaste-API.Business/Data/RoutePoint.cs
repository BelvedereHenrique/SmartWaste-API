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
    
    public partial class RoutePoint
    {
        public System.Guid ID { get; set; }
        public System.Guid RouteID { get; set; }
        public System.Guid PointID { get; set; }
    
        public virtual Point Point { get; set; }
        public virtual Route Route { get; set; }
    }
}
