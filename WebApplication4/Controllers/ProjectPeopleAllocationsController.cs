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
            //both PlanAHidden and PlanBHidden's default is "",so the defualt display student grade list will
            //hidden no thing, it will display whole list
            ViewBag.course = db.Course.ToList();
            ViewBag.plancs = db.PlanCourses.ToList();
            List<StudentInfo> StdInfom = GetStudents();
            ViewBag.stcs = StdInfom;
            //this is a list to store students information form different table

            ViewBag.stdt = db.Students.ToList();
    
            ViewBag.namedesc = "asc";
            //the namedesc is to make sure which button has be clicked

            
            
            var ddlyear = new List<string>();
            var DDLSemester = new List<string>();
            //create lists to put semester in.
            var currentDate = System.DateTime.Now;
            //create lists to put years in.

            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());
                //add years, Based on the current time

            }
            
            
            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
            //Find the location in the list where the current time is stored.
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
            //to judge which semester users are in now
            ViewBag.ddlyear = new SelectList(ddlyear, ddlyear[selectyear]);
            ViewBag.DDLSemester = new SelectList(DDLSemester, DDLSemester[selectMonth]);
            //Pass the time for the view, and the default is the current time
            var myeey = Convert.ToInt32(ddlyear[selectyear]);
            var sem = DDLSemester[selectMonth];
            ViewBag.project = db.Projects.Where(p => p.projectYear == myeey && p.projectSemester == sem).ToList();
            //In the view's project table, let it be automatically displayed as the project of the current time

            ViewBag.planid = 0;
            //The initial value is 0, which makes it display all by default.
            ViewBag.pop = "student";
            //pop is used to let the student or staff selection's defualt option is student.
            ViewBag.staffHidden = "hidden";
            //so , the staff list should be hidden, the staffHidden is as an attribute in staff's tag
            
            ViewBag.staff = db.Staff.ToList();           
            ViewBag.personID = db.AspNetUsers.ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();         
            ViewBag.planList = db.Plans.ToList();
            ViewBag.stdt = db.Students.ToList();

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            return View(projectPeopleAllocations.ToList());
        }
        [HttpPost]
        public ActionResult Index(int ddlyear, string DDLSemester, string namedesc, string pop, int plan, string tyt )
        {
            
            ViewBag.course = db.Course.ToList();
            ViewBag.plancs = db.PlanCourses.ToList();

            List<StudentInfo> StdInfom = GetStudents();
            ViewBag.stcs = StdInfom;

            ViewBag.tyt = tyt;
            //tyt is the sort selection's option's value
            ViewBag.namedesc = namedesc;
            //namedesc is to determine which button was pressed ,"asc" or "desc"
            ViewBag.planid = plan;     
            //planid is to find the selected plan's id 
            ViewBag.pop = pop;
            //pop is to find the students or staff selection's option's value.
            //These four viewbags(tyt namedesc planid pop) can also tell the view which option is selected,
            //instead of returning the first value or the default value when submitting the form.

            ViewBag.stdt = db.Students.ToList();
            ViewBag.planList = db.Plans.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();

            
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

                //Determine which button is pressed and sort it with the selected option.
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
            DDLSEM.Add(new SelectListItem { Text = "SP5", Selected = true });
            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");
            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");
            ViewBag.project = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).ToList();
            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Select(p => p.projectID).ToList();
            //Select the project of the corresponding time again.
            //The value of the selected time comes from the year and semeter drop-down boxes in view.

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Where(p => projects.Contains(p.projectID)).Include(p => p.Projects);
            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }


        public void AddStudent(int studentId, int projectId)   //Enter the student's id and project id as parameters for retrieval
        {

            var pro = new ProjectPeopleAllocations();     //Create a projectPeopleAllocations instance object named Pro
            pro.projectID = projectId;               //Assign values to the parameters in the instance object in turn.
            pro.personID = studentId;
            pro.personRole = "student";            //Name the role student.
            pro.dateCreated = DateTime.Now;        

            db.ProjectPeopleAllocations.Add(pro);          //Add instance objects with assigned values to the table.

            db.SaveChanges();        //Save changes
        }

        public void deletStudent(int studentId, int projectId)        //Enter the student's id and project id as parameters for retrieval
        {

            var pro = new ProjectPeopleAllocations();    //Create a projectPeopleAllocations instance object named Pro.
            pro.projectID = projectId;           //Assign values to the parameters in the instance object in turn.
            pro.personID = studentId;
            pro.personRole = "student";       //Name the role student.

            var dd = db.ProjectPeopleAllocations.SqlQuery($"delete  from ProjectPeopleAllocations where personID={studentId} and projectID={projectId}");
            try
            {
                dd.Count();
            }
            catch (Exception)
            {


            }      //Create an original SQL query, the query returns the entities in this set, and then do a try catch exception handling to count the number of deleted students.
            //db.ProjectPeopleAllocations.Remove(pro);

            //db.SaveChanges();
        }

        public void AddStaff(int staffId, int projectId)   //Enter the staff's id and project id as parameters for retrieval
        {

            var projinfos = db.Projects.FirstOrDefault(p => p.projectID == projectId);   ///Create an initial database named projinfos.

            var staff = db.Staff.FirstOrDefault(p => p.staffID == staffId);  //Create an initial database named staff.
            staff.projnum += 1;         //Add one to the number of project in staff.
            if (projinfos.difficult == "yes") 
            {
                staff.diffnum += 1;
            }     //Judge the Difficulty parameter of the item in the database, and if it is equal to yes, add one to the Difficulty quantity.

            var pro = new ProjectPeopleAllocations();  //Create a projectPeopleAllocations instance object named Pro.
            pro.projectID = projectId;       //Assign values to the parameters in the instance object in turn
            pro.personID = staffId;
            pro.personRole = "staff";   //Name the role staff
            pro.dateCreated = DateTime.Now;

            db.ProjectPeopleAllocations.Add(pro);  //Add instance objects with assigned values to the table.

            db.SaveChanges();  //Save changes.
        }

        public void deletStaff(int staffId, int projectId)  ////Enter the staff's id and project id as parameters for retrieval
        {

            var projinfos = db.Projects.FirstOrDefault(p => p.projectID == projectId);  //Create an initial database named projinfos.

            var staff = db.Staff.FirstOrDefault(p => p.staffID == staffId);  //Create an initial database named staff.
            staff.projnum -= 1;    //Reduce the number of items of staff by one.
            if (projinfos.difficult == "yes")
            {
                staff.diffnum -= 1;
            }   //Judge the Difficulty parameter of the project in the database, and if it is equal to yes, reduce the number of Difficulty by one.
            var pro = new ProjectPeopleAllocations();   // Create a projectPeopleAllocations instance object named Pro.
            pro.projectID = projectId;   //Assign values to the parameters in the instance object in turn.
            pro.personID = staffId;
            pro.personRole = "staff";    //Name the role staff

            var dd = db.ProjectPeopleAllocations.SqlQuery($"delete  from ProjectPeopleAllocations where personID={staffId} and projectID={projectId}");
            try
            {
                dd.Count();
            }
            catch (Exception)
            {


            }  //Create an original SQL query, the query returns the entities in this set, and then do a try catch exception handling to count the number of deleted staff.
            db.SaveChanges();   //Save changes

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


        private List<StudentInfo> GetStudents()
        {//this is a funcation to create a list include students information
         //and the "StudentInfo" is a new class what is written at the end of these controller
         //It is mainly used to extract data from different tables and bind them together
         
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
        {//this funcation is used to convert the numerical form of grades into string form
         //Greater than or equal to 90 is HD
         //Greater than or equal to 80, but less than 90 is D
         //.......
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
                return HttpNotFound();//If the ID is not found, a web page error is returned
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectPeopleAllocations).State = EntityState.Modified;//If the status of the entity is equal to the changed value
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
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
        //create a class to get data from different table
        //Each data type is the same as the data type what will be got
        //It has no meaning in itself. 
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
