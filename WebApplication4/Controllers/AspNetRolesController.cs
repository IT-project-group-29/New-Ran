using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using Newtonsoft.Json;

namespace WebApplication4.Controllers
{
    public class AspNetRolesController : Controller
    {
        private Model1 db = new Model1();

        // GET: AspNetRoles
        public ActionResult Index()
        {
            //create new object
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.Role = new SelectList(db.AspNetRoles, "Id", "Name");
            return View(db.AspNetRoles.ToList());
        }

        [HttpPost]
        public ActionResult Index(string firstname)
        {
            if (!string.IsNullOrEmpty(firstname))//Test whether the string is a nullnothingnullptr null reference or whether its value is empty
            {//don't empty
                ViewBag.User = new SelectList(db.AspNetUsers.Where(p => p.firstName == firstname), "Id", "UserName");
                //Returns the found user
            }
            else
            {
                ViewBag.User = new SelectList(db.AspNetUsers, "Id", "UserName");
            }

            //Transfer parameters
            ViewBag.firstname = firstname;
            ViewBag.Role = new SelectList(db.AspNetRoles, "Id", "Name");
            return View(db.AspNetRoles.ToList());
        }


        public ActionResult AddUserRole(string User, string Role)
            //add users and roles
        {
            using (var context = new Model1())
            {

                var ssss = db.AspNetUserRoles.SqlQuery("select * from AspNetUserRoles");
                //Displays all information in the aspnetuserroles table
                var ur = ssss.FirstOrDefault(p => p.UserId == User && p.RoleId == Role);
                //Returns the first element in the sequence; If the sequence does not contain any elements, the default value is returned.
                if (ur == null)
                {

                    var posts = context.Database.ExecuteSqlCommand($"insert into AspNetUserRoles(UserId,RoleId) values('{User}','{Role}') ");
                    //Add SQL statements to the database: add the values of Uesr and role to userid and roleid
                }
            }

            return RedirectToAction("Index");//Redirect page index
        }

        public JsonResult GetUserRole(string User)
        {
            var ssss = db.AspNetUserRoles.SqlQuery("select * from AspNetUserRoles");
            //Displays all information in the aspnetuserroles table
            var ur = ssss.Where(p => p.UserId == User).Select(p => p.RoleId).ToList();
            //Find the specified user and return the role
            var roles = db.AspNetRoles.Where(p => ur.Contains(p.Id)).ToList();
            //Match the roles of the current user with all roles
            var dic = new Dictionary<string, string>();
            foreach (var item in roles)
            {
                dic.Add(item.Id, item.Name);
                //Add the role of the current user to a new dictionary
            }
            return Json(dic);
        }
        public JsonResult RemoveUserRole(string User, string Role)
        {
            using (var context = new Model1())
            {
                //Add a delete statement to the database to delete the user and its role to be deleted
                var posts = context.Database.ExecuteSqlCommand($"delete from AspNetUserRoles where UserId='{User}' and RoleId='{Role}'");
            }
            return GetUserRole(User);
        }
        // GET: AspNetRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //An error request is returned. The ID cannot be empty
            }
            //Show all the values of the found ID
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
                //If the ID is not found, a web page error is returned
            }
            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]//Secure parameter transfer method
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] AspNetRoles aspNetRoles)
        //Only name authentication is allowed
        {
            if (ModelState.IsValid)
            //If the properties defined by the model are met
            {
                db.AspNetRoles.Add(aspNetRoles);
                //Create a new user
                db.SaveChanges();
                return RedirectToAction("Index");
                //Redirect page index
            }

            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //An error request is returned. The ID cannot be empty
            }
            //Show all the values of the found ID
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);//Create a database
            if (aspNetRoles == null)
            {
                return HttpNotFound();
                //If the ID is not found, a web page error is returned
            }
            return View(aspNetRoles);
        }


        [HttpPost]//Secure parameter transfer method
        [ValidateAntiForgeryToken]//Prevent cross-site request forgery
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        //Only name and id authentication is allowed
        {
            if (ModelState.IsValid)
            //If the properties defined by the model are met
            {
                db.Entry(aspNetRoles).State = EntityState.Modified;
                //Apply current changes to all columns
                db.SaveChanges();
                return RedirectToAction("Index");
                //Redirect page index
            }
            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //An error request is returned. The ID cannot be empty
            }
            //Show all the values of the found ID
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
                //If the ID is not found, a web page error is returned
            }
            return View(aspNetRoles);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            //Find the object corresponding to ID
            db.AspNetRoles.Remove(aspNetRoles);
            //remove found objects
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //Closing a form with dispose can release some of the resources occupied by the form, 
            //and the form closed with dispose can be recovered with show.
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
