namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectEfforts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectEfforts()
        {
            ProjectEffortsApplied = new HashSet<ProjectEffortsApplied>();
        }

        [Key]
        public int effortID { get; set; }

        [Required]
        [StringLength(100)]
        public string effortDescription { get; set; }

        public int effortRankValue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectEffortsApplied> ProjectEffortsApplied { get; set; }
    }
}
