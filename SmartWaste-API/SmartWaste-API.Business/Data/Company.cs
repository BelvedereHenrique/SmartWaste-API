
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
    
public partial class Company
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Company()
    {

        this.CompanyAddresses = new HashSet<CompanyAddress>();

        this.People = new HashSet<Person>();

        this.Routes = new HashSet<Route>();

        this.EmployeeCompanyRequests = new HashSet<EmployeeCompanyRequest>();

    }


    public System.Guid ID { get; set; }

    public string Name { get; set; }

    public string CNPJ { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Person> People { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Route> Routes { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<EmployeeCompanyRequest> EmployeeCompanyRequests { get; set; }

}

}
