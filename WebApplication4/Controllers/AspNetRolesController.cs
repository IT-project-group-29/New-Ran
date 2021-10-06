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
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.Role = new SelectList(db.AspNetRoles, "Id", "Name");
            return View(db.AspNetRoles.ToList());
        }


        public ActionResult AddUserRole(string User,string Role)
        {
            using (var context = new Model1())
            {

                var ssss = db.AspNetUserRoles.SqlQuery("select * from AspNetUserRoles");
                var ur = ssss.FirstOrDefault(p => p.UserId == User && p.RoleId == Role);
                if (ur == null)
                {

                    var posts = context.Database.ExecuteSqlCommand($"insert into AspNetUserRoles(UserId,RoleId) values('{User}','{Role}') ");

                }
            }

            return RedirectToAction("Index");
        }

        public JsonResult GetUserRole(string User)
        {
            var ssss = db.AspNetUserRoles.SqlQuery("select * from AspNetUserRoles");
            var ur = ssss.Where(p => p.UserId == User).Select(p=>p.RoleId).ToList();
            var roles=db.AspNetRoles.Where(p => ur.Contains(p.Id)).ToList();
            var dic=new Dictionary<string,string>();
            foreach (var item in roles)
            {
                dic.Add(item.Id, item.Name);
            }
            return  Json(dic);
        }
        public JsonResult RemoveUserRole(string User,string Role)
        {
            using (var context = new Model1())
            {

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
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                db.AspNetRoles.Add(aspNetRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: AspNetRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetRoles);
        }

        // GET: AspNetRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspNetRoles);
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
