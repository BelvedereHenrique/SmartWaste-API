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
    
    public partial class Identification
    {
        public System.Guid ID { get; set; }
        public string Value { get; set; }
        public int IdentificationTypeID { get; set; }
        public System.Guid PersonID { get; set; }
    
        public virtual IdentificationType IdentificationType { get; set; }
        public virtual Person Person { get; set; }
    }
}
