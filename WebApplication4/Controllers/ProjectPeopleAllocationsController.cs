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
    public class ProjectPeopleAllocationsController : Controller
    {
        
        private Model1 db = new Model1();

        // GET: ProjectPeopleAllocations
        public ActionResult Index()
        {
            ViewBag.PlanAHidden = "";
            ViewBag.PlanBHidden = "";
            ViewBag.course = db.Course.ToList();
            ViewBag.plancs = db.PlanCourses.ToList();
            List<StudentInfo> StdInfom = GetStudents();


            ViewBag.stdt = db.Students.ToList();
    
            ViewBag.namedesc = "asc";
            //ViewBag.stcs = db.StudentCourses.ToList();
            ViewBag.stcs = StdInfom;
            var ddlyear = new List<string>();
            var currentDate = System.DateTime.Now;


            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemester = new List<string>();
            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
            DDLSemester.Add("SP2");
            DDLSemester.Add("SP5");
            int selectMonth = 0;
            if (DateTime.Now.Month < 6)
            {
                selectMonth = 0;
            }
            else
            {
                selectMonth = 1;
            }
            ViewBag.planid = 0;      
            ViewBag.pop = "student";
            ViewBag.ddlyear = new SelectList(ddlyear, ddlyear[selectyear]);
            ViewBag.DDLSemester = new SelectList(DDLSemester,DDLSemester[selectMonth]);
            ViewBag.staff = db.Staff.ToList();
            ViewBag.staffHidden = "hidden";
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            ViewBag.personID = db.AspNetUsers.ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();
            var myeey = Convert.ToInt32(ddlyear[selectyear]);
            var sem = DDLSemester[selectMonth];
            ViewBag.project = db.Projects.Where(p => p.projectYear == myeey && p.projectSemester == sem).ToList();
            ViewBag.planList = db.Plans.ToList();
            ViewBag.stdt = db.Students.ToList();

            return View(projectPeopleAllocations.ToList());
        }
        [HttpPost]
        public ActionResult Index(int ddlyear, string DDLSemester, string namedesc, string pop, int plan, string tyt )
        {
            
            ViewBag.course = db.Course.ToList();
            ViewBag.plancs = db.PlanCourses.ToList();
            List<StudentInfo> StdInfom = GetStudents();            
            ViewBag.tyt = tyt;
            ViewBag.namedesc = namedesc;
            ViewBag.planid = plan;
            ViewBag.stcs = StdInfom;
            ViewBag.pop = pop;
            ViewBag.stdt = db.Students.ToList();
            ViewBag.planList = db.Plans.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.project = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).ToList();
            if(plan == 0)
            {
                ViewBag.PlanAHidden = "";
                ViewBag.PlanBHidden = "";

            }
            else if(plan == 1)
            {
                ViewBag.PlanAHidden = "";
                ViewBag.PlanBHidden = "hidden";
            }
            else if (plan == 2)
            {
                ViewBag.PlanBHidden = "";
                ViewBag.PlanAHidden = "hidden";
            }
            if (pop != "student")
                {
                    ViewBag.staffHidden = "";
                    ViewBag.studenthidden = "hidden";
                }
                else
                {
                    ViewBag.staffHidden = "hidden";
                    ViewBag.studenthidden = "";
                }
            
            
            var ddlyearlist = new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemesterlist = new List<string>();

            List<SelectListItem> DDLSEM = new List<SelectListItem>();
            DDLSEM.Add(new SelectListItem { Text = "SP2" });
            DDLSEM.Add(new SelectListItem { Text = "SP5" ,Selected = true});
            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");


           


            if (namedesc == "asc" )
            {
                if (tyt == "name")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.Name);
                    ViewBag.staff = db.Staff.OrderBy(p => p.username).ToList();
                }
                if(tyt == "gpa")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.Gpa);
                    
                }
                if (tyt == "105295")
                {
                    
                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.CPP);
                    
                }
                if (tyt == "105294")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.PF);
                    
                }
                if (tyt == "156783")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.WEB);
                    
                }
                if (tyt == "012540")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.IDIE);
                    
                }
                if (tyt == "105284")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.AgNET);
                    
                }




            }
            if (namedesc == "desc")
            {
                if (tyt == "name")
                {
                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.Name);
                    ViewBag.staff = db.Staff.OrderByDescending(p => p.username).ToList();
                }
                if (tyt == "gpa")
                {
                    ViewBag.stcs = StdInfom.OrderBy(p => p.Gpa);
               
                }
                if (tyt == "105295")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.CPP);
                    
                }
                if (tyt == "105294")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.PF);
                  
                }
                if (tyt == "156783")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.WEB);
                   
                }
                if (tyt == "012540")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.IDIE);
                   
                }
                if (tyt == "105284")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.AgNET);
                   
                }


            }


        



            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");

            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Select(p => p.projectID).ToList();

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Where(p => projects.Contains(p.projectID)).Include(p => p.Projects);

            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }


        public void AddStudent(int studentId, int projectId)
        {

            var pro = new ProjectPeopleAllocations();
            pro.projectID = projectId;
            pro.personID = studentId;
            pro.personRole = "student";
            pro.dateCreated = DateTime.Now;

            db.ProjectPeopleAllocations.Add(pro);

            db.SaveChanges();
        }

        public void deletStudent(int studentId, int projectId)
        {

            var pro = new ProjectPeopleAllocations();
            pro.projectID = projectId;
            pro.personID = studentId;
            pro.personRole = "student";

            var dd = db.ProjectPeopleAllocations.SqlQuery($"delete  from ProjectPeopleAllocations where personID={studentId} and projectID={projectId}");
            try
            {
                dd.Count();
            }
            catch (Exception)
            {


            }
            //db.ProjectPeopleAllocations.Remove(pro);

            //db.SaveChanges();
        }

        public void AddStaff(int staffId, int projectId)
        {

            var projinfos = db.Projects.FirstOrDefault(p => p.projectID == projectId);

            var staff = db.Staff.FirstOrDefault(p => p.staffID == staffId);
            staff.projnum += 1;
            if (projinfos.difficult == "yes")
            {
                staff.diffnum += 1;
            }

            var pro = new ProjectPeopleAllocations();
            pro.projectID = projectId;
            pro.personID = staffId;
            pro.personRole = "staff";
            pro.dateCreated = DateTime.Now;

            db.ProjectPeopleAllocations.Add(pro);

            db.SaveChanges();
        }

        public void deletStaff(int staffId, int projectId)
        {

            var projinfos = db.Projects.FirstOrDefault(p => p.projectID == projectId);

            var staff = db.Staff.FirstOrDefault(p => p.staffID == staffId);
            staff.projnum -= 1;
            if (projinfos.difficult == "yes")
            {
                staff.diffnum -= 1;
            }
            var pro = new ProjectPeopleAllocations();
            pro.projectID = projectId;
            pro.personID = staffId;
            pro.personRole = "staff";

            var dd = db.ProjectPeopleAllocations.SqlQuery($"delete  from ProjectPeopleAllocations where personID={staffId} and projectID={projectId}");
            try
            {
                dd.Count();
            }
            catch (Exception)
            {


            }
            db.SaveChanges();

        }

        // GET: ProjectPeopleAllocations/Details/5
        public ActionResult Details(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);

            projectPeopleAllocations.Projects = db.Projects.FirstOrDefault(p => p.projectID == projectID);

            projectPeopleAllocations.Users = db.AspNetUsers.FirstOrDefault(p => p.personID == personID);
            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Create
        public ActionResult Create()
        {
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectTitle");
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName");
            return View();
        }

        // POST: ProjectPeopleAllocations/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        private List<StudentInfo> GetStudents()
        {
            List<StudentInfo> StdInfom = new List<StudentInfo>();
            foreach (var item in db.Students)
            {
                var CPP = "";
                var PF = "";
                var WEB = "";
                var IDIE = "";
                var AgNET = "";

                foreach (var csgo in db.StudentCourses)
                {
                    if (csgo.studentID == item.studentID && csgo.courseID == "105295")
                    {
                        CPP = csgo.grade;
                    }
                }

                foreach (var csgo in db.StudentCourses)
                {
                    if (csgo.studentID == item.studentID && csgo.courseID == "105294")
                    {

                        PF = csgo.grade;
                    }
                }

                foreach (var csgo in db.StudentCourses)
                {
                    if (csgo.studentID == item.studentID && csgo.courseID == "156783")
                    {

                        WEB = csgo.grade;
                    }
                }
                foreach (var csgo in db.StudentCourses)
                {
                    if (csgo.studentID == item.studentID && csgo.courseID == "012540")
                    {

                        IDIE = csgo.grade;
                    }
                }
                foreach (var csgo in db.StudentCourses)
                {
                    if (csgo.studentID == item.studentID && csgo.courseID == "105284")
                    {

                        AgNET = csgo.grade;
                    }
                }






                    StdInfom.Add(new StudentInfo
                    {
                        ID = item.studentID,
                        Planid = item.planId,
                        Name = item.uniUserName,
                        Gpa = item.gpa,


                        CPP = CPP,
                        CPPtoStr = Gradetostring(CPP),
                        PF = PF,
                        PFtoStr = Gradetostring(PF),
                        WEB = WEB,
                        WEBtoStr = Gradetostring(WEB),
                        IDIE = IDIE,
                        IDIEtoStr = Gradetostring(IDIE),
                        AgNET = AgNET,
                        AgNETtoStr = Gradetostring(AgNET)



                    });;



                
                
            }
            return StdInfom;
        }
    
 public string Gradetostring(string grade)
        {
            string gradetoString = "";
            if (grade != "") { 
            if(Convert.ToInt32(grade)  >= 90)
            {
                gradetoString = "HD";
            }else if(Convert.ToInt32(grade) >= 80 && Convert.ToInt32(grade) < 90)
            {
                gradetoString = "D";
            }
            else if (Convert.ToInt32(grade) >= 70 && Convert.ToInt32(grade) < 80)
            {
                gradetoString = "C";
            }
            else if (Convert.ToInt32(grade) >= 60 && Convert.ToInt32(grade) < 70)
            {
                gradetoString = "P1";
            }
            else if (Convert.ToInt32(grade) >= 50 && Convert.ToInt32(grade) < 60)
            {
                gradetoString = "P2";
            }
            else if (Convert.ToInt32(grade) >= 40 && Convert.ToInt32(grade) < 50)
            {
                gradetoString = "F1";
            }
            else if (Convert.ToInt32(grade) <40 )
            {
                gradetoString = "F2";
            }
            }
            return gradetoString;
        }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            var hhh = db.AspNetUsers.FirstOrDefault(p => p.UserName == User.Identity.Name);
            projectPeopleAllocations.creatorID = hhh.personID;

            projectPeopleAllocations.dateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {

                db.ProjectPeopleAllocations.Add(projectPeopleAllocations);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Edit/5
        public ActionResult Edit(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);
            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectPeopleAllocations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Delete/5
        public ActionResult Delete(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);

            projectPeopleAllocations.Projects = db.Projects.FirstOrDefault(p => p.projectID == projectID);
            projectPeopleAllocations.Users = db.AspNetUsers.FirstOrDefault(p => p.personID == personID);

            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int projectID, int personID)
        {
            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.personID == personID && p.projectID == projectID);
            db.ProjectPeopleAllocations.Remove(projectPeopleAllocations);
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
    public class StudentInfo
    {
        public int ID { set; get; }
        public int Planid { set; get; }
        public string Name { set; get; }
        public decimal? Gpa { set; get; }
        public string CPP { set; get; }
        public string CPPtoStr { get; set; }

        public string PF { set; get; }
        public string PFtoStr { get; set; }
        public string WEB { set; get; }
        public string WEBtoStr { get; set; }
        public string IDIE { set; get; }
        public string IDIEtoStr { get; set; }
        public string AgNET { set; get; }
        public string AgNETtoStr { get; set; }
    }
}
