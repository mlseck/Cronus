//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            this.audittrails = new HashSet<audittrail>();
            this.employeetimeperiods = new HashSet<employeetimeperiod>();
            this.favorites = new HashSet<favorite>();
            this.groups = new HashSet<group>();
            this.groups1 = new HashSet<group>();
        }
    
        public string employeeID { get; set; }
        [Display(Name = "First Name")]
        public string employeeFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string employeeLastName { get; set; }
        [Display(Name = "Min Hours")]
        public Nullable<int> employeeMinHours { get; set; }
        [Display(Name = "Max Hours")]
        public Nullable<int> employeeMaxHours { get; set; }
        public int employeePrivileges { get; set; }
        [Display(Name = "Email address")]
        public string employeeEmailAddress { get; set; }
        [Display(Name = "Password")]
        public string employeePwd { get; set; }
        public Nullable<int> employeeGroupManaged { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<audittrail> audittrails { get; set; }
        public virtual group group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employeetimeperiod> employeetimeperiods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favorite> favorites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<group> groups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<group> groups1 { get; set; }
    }
}
