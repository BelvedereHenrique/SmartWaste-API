
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
    
public partial class User
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User()
    {

        this.People = new HashSet<Person>();

        this.UserRoles = new HashSet<UserRole>();

    }


    public System.Guid ID { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string RecoveryToken { get; set; }

    public Nullable<System.DateTime> RecoveredOn { get; set; }

    public Nullable<System.DateTime> ExpirationDate { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Person> People { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserRole> UserRoles { get; set; }

}

}
