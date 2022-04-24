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



            List<string> viewBagTest = new List<string>();

            if (index == 0 || index == null)
            {
                viewBagTest = db.PlanCourses.Include(a => a.Course).Select(a => a.Course).Select(a => a.courseName).ToList();
            }
            else
            {
                var test2 = db.PlanCourses.Include(a => a.Course).Where(a => a.planId == index).Select(a => a.Course).ToList();
                viewBagTest = test2.Select(a => a.courseName).ToList();
            }
            viewBagTest.Insert(0, "Name");
            ViewBag.Test = viewBagTest.ToList();

            var thisPlanStu = db.Students.Where(n => n.planId == index).ToList();
            List<MyViewModel> myList = new List<MyViewModel>();
            foreach (var n in thisPlanStu)
            {
                var courseStudents = db.StudentCourses.Where(m => m.studentID == n.studentID).ToList();
                var myModel = new MyViewModel
                {
                    Name = n.uniUserName,
                    StudentCourses = courseStudents
                };
                myList.Add(myModel);
            }
            ViewBag.MyList = myList.DistinctBy(a => a.Name).ToList();
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