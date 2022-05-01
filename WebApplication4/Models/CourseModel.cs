using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class CourseModel
    {
        public string Name { get; set; }

        public List<StudentCourses> StudentCourses { get; set; }
    }
}