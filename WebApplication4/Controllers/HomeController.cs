using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult Searchdata(string inputvalue)
        {
            inputvalue.ToLower();
            var searchInStudents = db.Students.Where(a => a.uniUserName.Contains(inputvalue)).ToList();
            var searchInStaffs = db.Staff.Where(a => a.username.Contains(inputvalue)).ToList();
            var searchInProjects = db.Projects.Where(a => a.projectTitle.Contains(inputvalue)).ToList();
            List<FunctionList> searchInPages = GetFunctions().Where(a => a.FunctionName.Contains(inputvalue)).ToList();
            var dataSwitch = new Dictionary<string, Dictionary<string,string>>();
            var students = new Dictionary<string, string>();
            var staffs = new Dictionary<string, string>();
            var projects = new Dictionary<string, string>();
            var pages = new Dictionary<string, string>();
            foreach (var item in searchInStudents)
            {
               
                students.Add(item.studentID.ToString(), item.uniUserName  );
                
            }
            foreach (var item in searchInStaffs)
            {
                
                staffs.Add(item.staffID.ToString(), item.username);
                
            }
            foreach (var item in searchInProjects)
            {
                
                projects.Add(item.projectID.ToString(), item.projectTitle);
                
            }
            foreach (var item in searchInPages)
            {

                pages.Add(item.Abbreviation, item.FunctionName);

            }
            dataSwitch.Add("stud", students);
            dataSwitch.Add("staf", staffs);
            dataSwitch.Add("proj", projects);
            dataSwitch.Add("page", pages);
            return Json(dataSwitch);
        }
        private List<FunctionList> GetFunctions()
        {
            var functions = new List<FunctionList>();

            functions.Add(new FunctionList { Abbreviation ="PJAL", FunctionName = "projects allocation" });
            functions.Add(new FunctionList { Abbreviation = "STFM", FunctionName = "staffs management" });
            functions.Add(new FunctionList { Abbreviation = "STDM", FunctionName = "students management" });
            functions.Add(new FunctionList { Abbreviation = "PJTM", FunctionName = "projects management" });
            functions.Add(new FunctionList { Abbreviation = "CSPL", FunctionName = "link courses to plans" });
            functions.Add(new FunctionList { Abbreviation = "CSPD", FunctionName = "courses-plans detail" });
            functions.Add(new FunctionList { Abbreviation = "SCSD", FunctionName = "srudents-courses detail" });
            return functions;

        }
    }
    public class FunctionList
    {
        public string Abbreviation { get; set; }    
        public string FunctionName { get; set; }    

    }
}