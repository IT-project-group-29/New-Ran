using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        private Model1 db = new Model1();


        public ActionResult Index()
        {
            //var staff = db.Staff.Include(a => a.AspNetUsers);
            return View(db.Staff.ToList());
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Email = db.AspNetUsers.FirstOrDefault(a => a.personID == id).Email;
            ViewBag.PhoneNumber = db.AspNetUsers.FirstOrDefault(a => a.personID == id).PhoneNumber;
            if (id == null)
            {
                return HttpNotFound();
            }
            Staff staff = (from a in db.Staff where a.staffID == id select a).FirstOrDefault();
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "staffID,uniStaffID,username,dateEnded,projnum,diffnum")] Staff staff, string email, string phoneNumber)
        {
            db.AspNetUsers.FirstOrDefault(a => a.personID == staff.staffID).Email = email;
            db.AspNetUsers.FirstOrDefault(a => a.personID == staff.staffID).PhoneNumber = phoneNumber;
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");    //back to the staffmanagement page after click the save button
            }
            return View(staff);
        }



        public ActionResult Detail(int id)
        {
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            ViewBag.uniStaffID = db.Staff.FirstOrDefault(a => a.staffID == id).uniStaffID;
            ViewBag.dateEnded = db.Staff.FirstOrDefault(a => a.staffID == id).dateEnded;
            return View(id);
        }


        public ActionResult Delete(int id)
        {
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            ViewBag.uniStaffID = db.Staff.FirstOrDefault(a => a.staffID == id).uniStaffID;
            ViewBag.dateEnded = db.Staff.FirstOrDefault(a => a.staffID == id).dateEnded;
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            AspNetUsers user = db.AspNetUsers.Where(a => a.personID == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            Staff staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}