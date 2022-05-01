using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class DetailedInformationController : Controller
    {
        private Model1 db = new Model1();
        // GET: DetailedInformation
        public ActionResult Index(int? index)
        {
            ViewBag.stdt = db.Students.ToList();
            ViewBag.plans = db.Plans.ToList();
            ViewBag.plansID = 0;
            ViewBag.course = db.Course.ToList();
            ViewBag.plancs = db.PlanCourses.ToList();
            List<StudentInfo> StdInfom = GetStudents();
            ViewBag.stcs = StdInfom;



            List<string> PlanCourses = new List<string>();

            if (index == 0 || index == null)
            {
                PlanCourses = db.PlanCourses.Include(a => a.Course).Select(a => a.Course).Select(a => a.courseName).ToList();
            }
            else
            {
                var plancourses = db.PlanCourses.Include(a => a.Course).Where(a => a.planId == index).Select(a => a.Course).ToList();
                PlanCourses = plancourses.Select(a => a.courseName).ToList();
            }
            PlanCourses.Insert(0, "Name");
            ViewBag.PlanCourse = PlanCourses.ToList();

            var thisPlanStudent = db.Students.Where(n => n.planId == index).ToList();
            List<CourseModel> CourseList = new List<CourseModel>();
            foreach (var n in thisPlanStudent)
            {
                var courseStudents = db.StudentCourses.Where(m => m.studentID == n.studentID).ToList();
                var myModel = new CourseModel
                {
                    Name = n.uniUserName,
                    StudentCourses = courseStudents
                };
                CourseList.Add(myModel);
            }
            ViewBag.CourseList = CourseList.DistinctBy(a => a.Name).ToList();
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);

            return View(projectPeopleAllocations.ToList());
        }

        private List<StudentInfo> GetStudents()
        {
            List<StudentInfo> StdInfom = new List<StudentInfo>();
            foreach (var item in db.Students)
            {
                StdInfom.Add(new StudentInfo
                {
                    ID = item.studentID,
                    Planid = item.planId,
                    Name = item.uniUserName,
                    Gpa = item.gpa,
                }); ;
            }
            return StdInfom;
        }



    }

}