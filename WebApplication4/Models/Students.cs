namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Students
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Students()
        {
            StudentCourses = new HashSet<StudentCourses>();
            StudentProjectRanking = new HashSet<StudentProjectRanking>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int studentID { get; set; }

        public int planId { get; set; }

        [Required]
        [StringLength(12)]
        public string uniUserName { get; set; }

        [Required]
        [StringLength(12)]
        public string uniStudentID { get; set; }

        public decimal? gpa { get; set; }

        [StringLength(5)]
        public string genderCode { get; set; }

        [StringLength(5)]
        public string international { get; set; }

        public bool externalStudent { get; set; }

        [StringLength(100)]
        public string studentEmail { get; set; }

        public int year { get; set; }

        [Required]
        [StringLength(3)]
        public string semester { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateEnded { get; set; }

        public virtual Plans Plans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentCourses> StudentCourses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentProjectRanking> StudentProjectRanking { get; set; }
    }
}
