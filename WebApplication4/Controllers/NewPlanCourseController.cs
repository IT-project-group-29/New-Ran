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
            
            ViewBag.plans = db.Plans.OrderBy(a => a.planName).ToList();
            ViewBag.course = db.Course.OrderBy(a => a.courseName).ToList();
            return View(db.PlanCourses.OrderBy(a => a.Course.courseName).ToList());
        }
        
        public ActionResult DeleteCourse(string DelcsID)
        {
            var pcDel = db.PlanCourses.Where(a => a.courseId == DelcsID).ToList();
            foreach (var item in pcDel)
            {
                item.isHidden = "hidden";

            }
            db.Course.FirstOrDefault(a => a.courseID == DelcsID).isHidden = "hidden";
            db.SaveChanges();

            ViewBag.plans = db.Plans.OrderBy(a => a.planName).ToList();
            ViewBag.course = db.Course.OrderBy(a => a.courseName).ToList();
            return PartialView("dragdrop", db.PlanCourses.OrderBy(a => a.Course.courseName).ToList());
        }

        public ActionResult DelPlan(int thisid)
        {
            var pcDel = db.PlanCourses.Where(a => a.planId == thisid).ToList();
            foreach (var item in pcDel)
            {
                item.isHidden="hidden";
            }
            var stud = db.Students.Where(a => a.planId == thisid).ToList();
            foreach(var item in stud)
            {
                item.planId = 0;
               // item.planId = null;
            }
            db.Plans.FirstOrDefault(a => a.planId == thisid).isHidden="hidden";
            db.SaveChanges();
            return Content("removed");
        }
        public ActionResult DelSpan(int thisid, string deld)
        {
            int pID = thisid;
            string cID = deld.Substring((deld.IndexOf("bind") + 5));
            var pc = db.PlanCourses.FirstOrDefault(a => a.planId == thisid && a.courseId == cID);

            pc.isHidden = "hidden";
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
        public ActionResult IsCourseNameRepeated(string courseName)
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

        public ActionResult IsCourseCodeRepeated(string courseCode)
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
        public ActionResult IsPlanNameRepeated(string planName)
        {
            if (planName != "")
            {

                if (db.Plans.FirstOrDefault(a => a.planName == planName) == null)
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
        public ActionResult IsPlanCodeRepeated(string planCode)
        {
            if (planCode != "")
            {

                if (db.Plans.FirstOrDefault(a => a.planCode == planCode) == null)
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

            
            var newCourse = new Course();
            newCourse.courseID = CId;
            newCourse.courseName = CName;
            newCourse.courseCode = CCode;
            db.Course.Add(newCourse);
            db.SaveChanges();
            return Content("success");
        }

        public ActionResult AddPlan(string PName,string PCode,string PDur)
        {
            
            var newPlan = new Plans();
            newPlan.planName = PName;
            newPlan.planCode = PCode;
            newPlan.projectDuration = Convert.ToInt32(PDur);
            db.Plans.Add(newPlan);
            db.SaveChanges();
            ViewBag.plans = db.Plans.OrderBy(a => a.planName).ToList();
            ViewBag.course = db.Course.OrderBy(a => a.courseName).ToList();
            return PartialView("dragdrop", db.PlanCourses.ToList());

        }
        public ActionResult AddCourseByAjaxForm(string CId,string CName, string CCode)
        {

            var newCourse = new Course();
            newCourse.courseID = CId;
            newCourse.courseName = CName;
            newCourse.courseCode = CCode;
            db.Course.Add(newCourse);
            db.SaveChanges();
            ViewBag.course = db.Course.OrderBy(a => a.courseName).ToList();
            return PartialView("AddNew");
        }

        /*=============================================================================*/

        public ActionResult CourseAndPlan()
        {
            ViewBag.course = db.Course.OrderBy(a => a.courseCode).ToList();
            return View();
        }

        public ActionResult HiddenOrNot(string choose)
        {
            ViewBag.course = db.Course.OrderBy(a => a.courseCode).ToList();
            if (choose == "notHidden")
            {

                return PartialView("notHidden");
            }
            else
            {
                return PartialView("HiddenCourse");
            }
        }
    }
}