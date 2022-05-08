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
        [Display(Name ="Course ID")]
        [Required]
        [StringLength(10,ErrorMessage ="Course ID should no longer than 10")]
        public string courseID { get; set; }
        [Display(Name = "Course Name")]
        [Required]
        [StringLength(150,ErrorMessage = "Course Name should no longer than 150")]
        public string courseName { get; set; }
        [Display(Name = "Course Code")]
        [Required]
        [StringLength(10,ErrorMessage ="Course Code should no longer than 10")]
        public string courseCode { get; set; }

        [StringLength(6)]
        public string courseAbbreviation { get; set; }


        public string isHidden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCourses> PlanCourses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentCourses> StudentCourses { get; set; }
    }
}
