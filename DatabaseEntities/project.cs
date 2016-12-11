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
    using System.Web.Mvc;

    public partial class project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public project()
        {
            this.hoursworkeds = new HashSet<hoursworked>();
            this.activities = new HashSet<activity>();
            this.groups = new HashSet<group>();
        }

        public int projectID { get; set; }
        [Display(Name = "Project Name")]
        public string projectName { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> projectStartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> projectEndDate { get; set; }
        [Display(Name = "Description")]
        public string projectDescription { get; set; }
        [Display(Name = "Capital Code")]
        public string projectCapitalCode { get; set; }
        [Display(Name = "Abbreviation")]
        public string projectAbbreviation { get; set; }
        [Display(Name = "Status")]
        public short projectActive { get; set; }

        public string projectActiveString {
            get
            {
                return projectActive.ToString();
            }
            set
            {
                projectActive = short.Parse(value);
            }
        }

        [Display(Name = "Status")]
        public List<SelectListItem> status;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hoursworked> hoursworkeds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<activity> activities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<group> groups { get; set; }
    }
}
