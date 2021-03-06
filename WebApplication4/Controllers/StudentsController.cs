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
    public class StudentsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Students
        public ActionResult Index(string searchStudent = "")
        {
            var students = db.Students.Include(s => s.Plans);
            if (!string.IsNullOrEmpty(searchStudent))
            {
                students = students.Where(s => s.uniUserName.Contains(searchStudent));
            }
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "studentID,planId,uniUserName,uniStudentID,gpa,genderCode,international,externalStudent,studentEmail,year,semester,dateEnded")] Students students)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", students.planId);
            return View(students);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//An error request is returned. The ID cannot be empty
            }
            Students students = db.Students.Find(id);//Create a database
            if (students == null)
            {
                return HttpNotFound();//If the ID is not found, a web page error is returned
            }
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", students.planId);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(students);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "studentID,planId,uniUserName,uniStudentID,gpa,genderCode,international,externalStudent,studentEmail,year,semester,dateEnded")] Students students)
        {
            if (ModelState.IsValid)
            {   
  
                db.Entry(students).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.planId = new SelectList(db.Plans, "planId", "planName", students.planId);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Students students = db.Students.Find(id);
            db.Students.Remove(students);
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
