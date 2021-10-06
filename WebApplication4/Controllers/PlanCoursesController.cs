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
    public class PlanCoursesController : Controller
    {
        private Model1 db = new Model1();

        // GET: PlanCourses
        public ActionResult Index()
        {
            var planCourses = db.PlanCourses.Include(p => p.Course).Include(p => p.Plans);
            return View(planCourses.ToList());
        }

        // GET: PlanCourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCourses planCourses = db.PlanCourses.Find(id);
            if (planCourses == null)
            {
                return HttpNotFound();
            }
            return View(planCourses);
        }

        // GET: PlanCourses/Create
        public ActionResult Create()
        {
            ViewBag.courseId = new SelectList(db.Course, "courseID", "courseName");
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName");
            return View();
        }

        // POST: PlanCourses/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "planId,courseId,temp")] PlanCourses planCourses)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PlanCourses.Add(planCourses);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                  
                }
               
                return RedirectToAction("Index");
            }

            ViewBag.courseId = new SelectList(db.Course, "courseID", "courseName", planCourses.courseId);
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", planCourses.planId);
            return View(planCourses);
        }

        // GET: PlanCourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCourses planCourses = db.PlanCourses.Find(id);
            if (planCourses == null)
            {
                return HttpNotFound();
            }
            ViewBag.courseId = new SelectList(db.Course, "courseID", "courseName", planCourses.courseId);
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", planCourses.planId);
            return View(planCourses);
        }

        // POST: PlanCourses/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "planId,courseId,temp")] PlanCourses planCourses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planCourses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.courseId = new SelectList(db.Course, "courseID", "courseName", planCourses.courseId);
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", planCourses.planId);
            return View(planCourses);
        }

        // GET: PlanCourses/Delete/5
        public ActionResult Delete(int? courid ,int? planid)
        {
            if (courid == null||planid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanCourses planCourses = db.PlanCourses.FirstOrDefault(p=>(p.planId==planid.Value)&&(p.courseId==courid.Value.ToString()));
            if (planCourses == null)
            {
                return HttpNotFound();
            }
            return View(planCourses);
        }

        // POST: PlanCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? courid, int? planid)
        {
            PlanCourses planCourses = db.PlanCourses.FirstOrDefault(p =>( p.planId == planid.Value )&& (p.courseId == courid.Value.ToString()));
            db.PlanCourses.Remove(planCourses);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
