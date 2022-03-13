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
    
    public class AllocationsController : Controller
    {

        private Model1 db = new Model1();
        // GET: Allocations
        public ActionResult Index()
        {

           
            ViewBag.ddlyear = DateNow();
            ViewBag.DDLSemester = SemNow();
            //Pass the time for the view, and the default is the current time

            var sem = SemNow();
            ViewBag.project = db.Projects.Where(p => p.projectYear == DateTime.Now.Year && p.projectSemester == "SP2").ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            ViewBag.plans = db.Plans.ToList();
            ViewBag.plansID = GetAllPlansID();
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            
            return View(projectPeopleAllocations.ToList());
        }
        public SelectList PlanSel()
        {
            var plans = new List<string>();
            return null;
        }
        public SelectList DateNow()
        {
            var ddlyear = new List<string>();
            
            //create lists to put semester in.
            var currentDate = System.DateTime.Now;
            //create lists to put years in.

            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());
                //add years, Based on the current time

            }


            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
            //Find the location in the list where the current time is stored.
          

            
            //to judge which semester users are in now
            return new SelectList(ddlyear, ddlyear[selectyear]);
        }
        public SelectList SemNow()
        {
            var DDLSemester = new List<string>();
            DDLSemester.Add("SP2");
            DDLSemester.Add("SP5");

            int selectMonth = 0;
            if (DateTime.Now.Month < 6)
            {
                selectMonth = 0;
            }
            else
            {
                selectMonth = 1;
            }
            return new SelectList(DDLSemester, DDLSemester[selectMonth]);
        }

        public List<int> GetAllPlansID()
        {//use this func can get all plans' ID in database
            var l = new List<int>();
            var a = db.Plans.ToList();
            foreach (var item in a)
            {
                l.Add(item.planId);
            }
            return l;
        }
       public List<string> Plan_course(int planid)
        {//use this func can get a List which store course ID by  planID
            var l = new List<string>();
            var a = db.PlanCourses.Where(p => p.planId == planid).ToList();
            foreach(var item in a)
            {
                l.Add(item.courseId);
            }
            return l;
        }
        public string CourseName(string courseId)
        {//get course name by course ID
            var a = db.Course.FirstOrDefault(p => p.courseID.Equals(courseId)).courseName;
            return a;
        }
        public ActionResult Change(string pop)
        {
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            if (pop == "staff")
            {
                
                return PartialView("staffTable");
            }
            else
            {
                return PartialView("AllPlanStudent");
            }
           
        }
        public ActionResult ChangeDate(int ddlyear, string DDLSemester)
        {
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            ViewBag.project = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            return PartialView("ProjectAllocation", projectPeopleAllocations.ToList());
        }

        public ActionResult EditStaff(int STID)
        {
            var a = db.Staff.FirstOrDefault(c => c.staffID == STID);
            ViewBag.stff = a;
            return PartialView("staffDiv");
        }
    }
}