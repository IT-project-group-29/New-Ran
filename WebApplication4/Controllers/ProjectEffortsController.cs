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
    public class ProjectEffortsController : Controller
    {
        private Model1 db = new Model1();

        // GET: ProjectEfforts
        public ActionResult Index()
        {
            return View(db.ProjectEfforts.ToList());
        }

        // GET: ProjectEfforts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectEfforts projectEfforts = db.ProjectEfforts.Find(id);
            if (projectEfforts == null)
            {
                return HttpNotFound();
            }
            return View(projectEfforts);
        }

        // GET: ProjectEfforts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectEfforts/Create
        // To prevent an "over publish" attack, enable the specific property to bind to.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "effortID,effortDescription,effortRankValue")] ProjectEfforts projectEfforts)
        {
            if (ModelState.IsValid)
            {
                db.ProjectEfforts.Add(projectEfforts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectEfforts);
        }

        // GET: ProjectEfforts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectEfforts projectEfforts = db.ProjectEfforts.Find(id);
            if (projectEfforts == null)
            {
                return HttpNotFound();
            }
            return View(projectEfforts);
        }

        // POST: ProjectEfforts/Edit/5
        // To prevent an "over publish" attack, enable the specific property to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "effortID,effortDescription,effortRankValue")] ProjectEfforts projectEfforts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectEfforts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectEfforts);
        }

        // GET: ProjectEfforts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectEfforts projectEfforts = db.ProjectEfforts.Find(id);
            if (projectEfforts == null)
            {
                return HttpNotFound();
            }
            return View(projectEfforts);
        }

        // POST: ProjectEfforts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectEfforts projectEfforts = db.ProjectEfforts.Find(id);
            db.ProjectEfforts.Remove(projectEfforts);
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
