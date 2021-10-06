using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class AspNetUsersController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private Model1 db = new Model1();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        //// GET: AspNetUsers/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
        //    if (aspNetUsers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetUsers);
        //}

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel aspNetUsers)
        {

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = aspNetUsers.UserName, firstName = aspNetUsers.firstName, lastName = aspNetUsers.lastName, title = aspNetUsers.title, Email = aspNetUsers.Email };
                var result =  UserManager.CreateAsync(user, "asdQWE123!@#");

                var huh=result.Result;
                if (huh.Succeeded)
                {
                    //db.AspNetUsers.Add(aspNetUsers);
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }

                AddErrors(result.Result);

            }
           
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(aspNetUsers);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        //// GET: AspNetUsers/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
        //    if (aspNetUsers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetUsers);
        //}

        //// POST: AspNetUsers/Edit/5
        //// 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        //// 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,personID,dateCreated,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,title,firstName,lastName,AccountAccepted")] AspNetUsers aspNetUsers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(aspNetUsers).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(aspNetUsers);
        //}

        //// GET: AspNetUsers/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
        //    if (aspNetUsers == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetUsers);
        //}

        //// POST: AspNetUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
        //    db.AspNetUsers.Remove(aspNetUsers);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
