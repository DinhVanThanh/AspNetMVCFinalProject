using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrivateTutorOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles = "Admin, Customer, Tutor")]
    public class ClassController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private PrivateTutorOnline.Models.TutorOnlineDBContext db = new Models.TutorOnlineDBContext();
        // GET: Class
        public ActionResult ClassRegistrationForm()
        {
            return View("ClassRegistrationForm", new Models.ViewModels.ClassRegistrationFormViewModel(){
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList()
            });
        }
        public ApplicationRoleManager AppRoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }
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
        [HttpPost]
        public ActionResult ClassRegister(Models.BindingModels.ClassRegistrationBindingModel classRegistrationInfo)
        {
            if(classRegistrationInfo != null)
            {
                string UserId = User.Identity.GetUserId();
                Models.RegistrationClass classInfo = new Models.RegistrationClass();
                classInfo.City = classRegistrationInfo.City;
                classInfo.DayPerWeek = classRegistrationInfo.SessionPerWeek;
                classInfo.District = classRegistrationInfo.District;
                classInfo.Grade = db.Grades.SingleOrDefault(s => s.Id == classRegistrationInfo.Grade);
                classInfo.Requirement = classRegistrationInfo.Requirement;
                classInfo.SalaryPerMonth = classRegistrationInfo.SalaryPerMonth;
                classInfo.Street = classRegistrationInfo.Street;
                classInfo.Status = Enums.ClassStatus.WaitingForAdminApproval;
                classInfo.Subjects = new List<Subject>();
                classInfo.Customer = db.Customers.SingleOrDefault(s => s.UserId == UserId);
                foreach (int id in classRegistrationInfo.Subjects)
                    classInfo.Subjects.Add(db.Subjects.SingleOrDefault(s => s.Id == id));
                classInfo.TutoringTime= classRegistrationInfo.TeachingTime;
                classInfo.Ward = classRegistrationInfo.Ward;
                classInfo.ReceivedDate = DateTime.Now;
                db.RegistrationClasses.Add(classInfo);
                db.SaveChanges();
                return RedirectToAction("PostedClass", "ManageRegistrationClasses");
            }
            return View("Error");
        }
    }
}