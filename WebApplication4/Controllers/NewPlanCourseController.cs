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
        public ActionResult ChangeTable(int plID,string csID,int PorC)
        {          
                //Thread.Sleep(100);
                ViewBag.plans = db.Plans.ToList();
                ViewBag.course = db.Course.ToList();
                var pcPlan = db.PlanCourses.Where(a => a.planId == plID).ToList();
                var pcCourse = db.PlanCourses.Where(a => a.courseId == csID).ToList();
                bool f = true;
                if (PorC == 0)
                {
                    foreach (var item in pcPlan)
                    {
                        if (item.courseId == csID)
                        {

                            f = false;
                        }
                    }
                    if (f)
                    {
                        var newpc = new PlanCourses();
                        newpc.courseId = csID;
                        newpc.planId = plID;
                        db.PlanCourses.Add(newpc);
                        db.SaveChanges();
                        return PartialView("dragdrop", db.PlanCourses.ToList());
                    }
                    return PartialView("dragdrop", db.PlanCourses.ToList());
                }
                else if(PorC == 1)
                {
                    foreach (var item in pcCourse)
                    {
                        if (item.planId == plID)
                        {
                            f = false;
                        }
                    }
                    if (f)
                    {
                        var newpc = new PlanCourses();
                        newpc.courseId = csID;
                        newpc.planId = plID;
                        db.PlanCourses.Add(newpc);
                        db.SaveChanges();
                        return PartialView("dragdrop", db.PlanCourses.ToList());
                    }
                    return PartialView("dragdrop", db.PlanCourses.ToList());
                }
            return PartialView("dragdrop", db.PlanCourses.ToList());
        }
        public ActionResult DeleteSpan(string SpanplID,string SpancsID)
        {
            ViewBag.plans = db.Plans.ToList();
            ViewBag.course = db.Course.ToList();
            int plID =  Convert.ToInt32( SpanplID.Substring(4));
            string csID = SpancsID;
            var pc = db.PlanCourses.FirstOrDefault(a => a.planId == plID && a.courseId == csID);

            db.PlanCourses.Remove(pc);
            db.SaveChanges();

           
            

            return PartialView("dragdrop", db.PlanCourses.ToList());
        }
    }
}