using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using System.Threading;

namespace WebApplication4.Controllers
{
    public class NewPlanCourseController : Controller
    {
        private Model1 db = new Model1();
        // GET: NewPlanCourse
        public ActionResult Index()
        {
            ViewBag.plans = db.Plans.ToList();
            ViewBag.course = db.Course.ToList();
            return View(db.PlanCourses.ToList());
        }

        public ActionResult DeleteCourse(string DelcsID)
        {
            ViewBag.plans = db.Plans.ToList();
            ViewBag.course = db.Course.ToList();
            var pcDel = db.PlanCourses.Where(a => a.courseId == DelcsID).ToList();
            foreach (var item in pcDel)
            {
                db.PlanCourses.Remove(item);

            }
            db.Course.Remove(db.Course.FirstOrDefault(a => a.courseID == DelcsID));
            db.SaveChanges();
            return PartialView("dragdrop", db.PlanCourses.ToList());
        }


        public ActionResult DelSpan(int thisid, string deld)
        {
            int pID = thisid;
            string cID = deld.Substring((deld.IndexOf("bind") + 5));
            var pc = db.PlanCourses.FirstOrDefault(a => a.planId == thisid && a.courseId == cID);

            db.PlanCourses.Remove(pc);
            db.SaveChanges();
            return Content("removed");
        }

        public ActionResult AddCoursePlan(int planId, string courseId)
        {
            var pcPlan = db.PlanCourses.Where(a => a.planId == planId).ToList();
            Boolean flag = true;
            foreach (var item in pcPlan)
            {
                if (item.courseId == courseId)
                {

                    flag = false;
                }
            }
            if (flag)
            {
                var newpc = new PlanCourses();
                newpc.courseId = courseId;
                newpc.planId = planId;
                db.PlanCourses.Add(newpc);
                db.SaveChanges();
                var courseName = db.Course.FirstOrDefault(a => a.courseID == courseId).courseName;
                return Content(courseName);
            }
            return Content("This course is already selected");
        }
        public ActionResult IsCourseIdRepeated(string courseId)
        {
            if (courseId != "")
            {
                if (db.Course.FirstOrDefault(a => a.courseID == courseId) == null)
                {
                    return Content("OK");
                }
                else
                {
                    return Content("repeated");
                }
            }
            else return Content("null");
        }
        public ActionResult IsCourseNameRepeated(String courseName)
        {
            if (courseName != "")
            {

                if (db.Course.FirstOrDefault(a => a.courseName == courseName) == null)
                {
                    return Content("OK");
                }
                else
                {
                    return Content("repeated");
                }

            }
            else { return Content("null"); }

        }

        public ActionResult IsCourseCodeRepeated(String courseCode)
        {

            if (courseCode != "")
            {

                if (db.Course.FirstOrDefault(a => a.courseCode == courseCode) == null)
                {
                    return Content("OK");
                }
                else
                {
                    return Content("repeated");
                }

            }
            else { return Content("null"); }
        }

        public ActionResult AddCourse(string CId,string CName, string CCode)
        {

            ViewBag.plans = db.Plans.ToList();
            ViewBag.course = db.Course.ToList();
            var newCourse = new Course();
            newCourse.courseID = CId;
            newCourse.courseName = CName;
            newCourse.courseCode = CCode;
            db.Course.Add(newCourse);
            db.SaveChanges();
            return Content("success");
        }
    }
}