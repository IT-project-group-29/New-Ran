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
            ViewBag.user = db.AspNetUsers.ToList();
            return View(db.Clients.ToList());
        }
        public ActionResult Detail(int id)
        {
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            ViewBag.companyName = db.Clients.FirstOrDefault(a => a.clientID == id).companyName;
            return View(id);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Email = db.AspNetUsers.FirstOrDefault(a => a.personID == id).Email;
            ViewBag.PhoneNumber = db.AspNetUsers.FirstOrDefault(a => a.personID == id).PhoneNumber;
            if (id == null)
            {
                return HttpNotFound();
            }
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
        {
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
            ViewBag.getPersonID = db.AspNetUsers.FirstOrDefault(a => a.personID == id);
            ViewBag.companyName = db.Clients.FirstOrDefault(a => a.clientID == id).companyName;
            Clients client = db.Clients.Find(id);
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
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}