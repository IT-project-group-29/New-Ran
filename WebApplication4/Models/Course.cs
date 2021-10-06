namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            PlanCourses = new HashSet<PlanCourses>();
            StudentCourses = new HashSet<StudentCourses>();
        }

        [StringLength(10)]
        public string courseID { get; set; }

        [StringLength(150)]
        public string courseName { get; set; }

        [StringLength(10)]
        public string courseCode { get; set; }

        [StringLength(6)]
        public string courseAbbreviation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCourses> PlanCourses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentCourses> StudentCourses { get; set; }
    }
}
