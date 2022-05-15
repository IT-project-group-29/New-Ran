using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ClientController : Controller
    {
        private Model1 db = new Model1();
        // GET: Client
        public ActionResult Index()
        {
            //get data from AspNetUsers table from database and send data to Index page.
            ViewBag.user = db.AspNetUsers.ToList();
            return View(db.Clients.ToList());
        }
        public ActionResult Detail(int id)//If user want to open detail page they need give an id.
        {
            //get all data of AspNetUsers table 
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            //get companyName from Client table
            ViewBag.companyName = db.Clients.FirstOrDefault(a => a.clientID == id).companyName;
            return View(id);
        }
        public ActionResult Edit(int? id)
        {
            //Get Email from AspNetUsers table
            ViewBag.Email = db.AspNetUsers.FirstOrDefault(a => a.personID == id).Email;
            //Get PhoneNumber from AspNetUsers table
            ViewBag.PhoneNumber = db.AspNetUsers.FirstOrDefault(a => a.personID == id).PhoneNumber;
            if (id == null)
            {
                return HttpNotFound();
            }
            //Get data of client which has same id passed.
            Clients client = (from a in db.Clients where a.clientID == id select a).FirstOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "clientID,companyName")] Clients client, string email, string phoneNumber)
        //The paramater passed is editted at Edit page.
        {
            //These are used to change email and phonenumber in AspNetUsers table in database by new data that user input at Edit page. 
            db.AspNetUsers.FirstOrDefault(a => a.personID == client.clientID).Email = email;
            db.AspNetUsers.FirstOrDefault(a => a.personID == client.clientID).PhoneNumber = phoneNumber;
            db.SaveChanges();
            if(ModelState.IsValid)
            {
                db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");    //back to the Index page after click the save button
            }
            return View(client);
        }

        public ActionResult Delete(int id)
        {
            //This is used to get data from AspNetUsers where has same personID
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            //This is used to get data from Clients where has same clientID
            ViewBag.companyName = db.Clients.FirstOrDefault(a => a.clientID == id).companyName;
            Clients client = db.Clients.Find(id);
            //Because we need to get an id first, after that to get in Delete page, 
            //so we need to check if we succefully get the id.
            if (client == null)
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
            Clients client = db.Clients.Find(id);
            db.Clients.Remove(client);
            //These are used to remove account at table in database.
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            //After delete account turn back to Index page.
            return RedirectToAction("Index");
        }
    }
}