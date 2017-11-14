using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorOnline.Controllers
{
    public class ClassController : Controller
    {
        private PrivateTutorOnline.Models.TutorOnlineDBContext db = new Models.TutorOnlineDBContext();
        // GET: Class
        public ActionResult ClassRegistrationForm()
        {
            return View("ClassRegistrationForm", new Models.ViewModels.ClassRegistrationFormViewModel(){
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList()
            });
        }
        [HttpPost]
        public ActionResult ClassRegister(Models.BindingModels.ClassRegistrationBindingModel classRegistrationInfo)
        {
            if(classRegistrationInfo != null)
            {
                Models.RegistrationClass classInfo = new Models.RegistrationClass();
                classInfo.City = classRegistrationInfo.City;
                classInfo.DayPerWeek = classRegistrationInfo.SessionPerWeek;
                classInfo.District = classRegistrationInfo.District;
                classInfo.Grade = classRegistrationInfo.Grade;
                classInfo.Requirement = classRegistrationInfo.Requirement;
                classInfo.SalaryPerMonth = classRegistrationInfo.SalaryPerMonth;
                classInfo.Street = classRegistrationInfo.Street;
                foreach(int id in classRegistrationInfo.Subjects)
                    classInfo.Subjects.Add(db.Subjects.SingleOrDefault(s => s.Id == id));
                classInfo.TutoringTime= classRegistrationInfo.TeachingTime;
                classInfo.Ward = classRegistrationInfo.Ward;
                classInfo.TutorId = 1;
                classInfo.CustomerId = 1;
                return View("Success");
            }
            return View("Error");
        }
    }
}