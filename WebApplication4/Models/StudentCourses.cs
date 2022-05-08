namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentCourses : IComparable<StudentCourses>
    {
        [Key]
        public int studentCourseID { get; set; }

        public int studentID { get; set; }

        [Required]
        [StringLength(10)]
        public string courseID { get; set; }

        [StringLength(5)]
        public string semester { get; set; }

        public int? year { get; set; }

        [StringLength(5)]
        public string grade { get; set; }

        public decimal? mark { get; set; }

        public virtual Course Course { get; set; }

        public virtual Students Students { get; set; }

        public int CompareTo(StudentCourses studentCourses)
        {
            if (studentCourses.grade == this.grade)
            {
                return 0;
            }
            else if (int.Parse(studentCourses.grade) > int.Parse(this.grade))
            {
                return -1;
            }
            else if (int.Parse(studentCourses.grade) < int.Parse(this.grade))
            {
                return 1;
            }
            return 0;
        }
    }
}
