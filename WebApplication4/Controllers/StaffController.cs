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
            return View(db.Staff.ToList());
        }

        public ActionResult Edit(int? id)
        {
            //Get Email and PhoneNumber from AspNetUsers table
            ViewBag.Email = db.AspNetUsers.FirstOrDefault(a => a.personID == id).Email;
            ViewBag.PhoneNumber = db.AspNetUsers.FirstOrDefault(a => a.personID == id).PhoneNumber;
            if (id == null)
            {
                return HttpNotFound();
            }
            //Get data of staff which has same id passed.
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
        //The paramater passed is editted at Edit page.
        {
            //These are used to change email and phonenumber in AspNetUsers table in database by new data that user input at Edit page. 
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



        public ActionResult Detail(int id)//If user want to open detail page they need give an id.
        {
            //get all data of AspNetUsers table
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            //Get uniStaffID and dateEnded from Staff table
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
            //Because we need to get an id first, after that to get in Delete page, 
            //so we need to check if we succefully get the id.
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
            //These are used to remove account at table in database.
            db.Staff.Remove(staff);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            //After delete account turn back to Index page.
            return RedirectToAction("Index");
        }
    }
}