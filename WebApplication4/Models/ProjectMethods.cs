namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectMethods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectMethods()
        {
            ProjectMethodsApplied = new HashSet<ProjectMethodsApplied>();
        }

        [Key]
        public int methodID { get; set; }

        [Required]
        [StringLength(100)]
        public string methodDescription { get; set; }

        public bool otherDetailFlag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMethodsApplied> ProjectMethodsApplied { get; set; }
    }
}
