using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateTutorOnline.Models;
using System.IO;
using PrivateTutorOnline.Models.BindingModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PrivateTutorOnline.Models.ViewModels;
using PagedList;
using PrivateTutorOnline.Enums;
using PrivateTutorOnline.Services;
using System.Configuration;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles = "Customer, Admin, Tutor, Owner")]
    public class TutorsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager; 
        private TutorOnlineDBContext db = new TutorOnlineDBContext();
        private string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
        public TutorsController()
        {

        }
        public TutorsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        public JsonResult ChooseTutor(int TutorId)
        {
            
            string UserId = User.Identity.GetUserId();
            string postedClasses = "";
            Customer customer = db.Customers.SingleOrDefault(s => s.UserId == UserId);
            Tutor tutor = db.Tutors.SingleOrDefault(s => s.Id == TutorId);
            IList<RegistrationClass> Classes = db.RegistrationClasses.Include(t => t.Subjects).Include(t => t.Grade).Include(t => t.Customer).Where(t => t.Customer.UserId == UserId && (t.Status == ClassStatus.AdminApproved || t.Status == ClassStatus.CustomerRejected)).ToList();
           
            
            foreach (var item in Classes)
            { 
                try
                {
                    EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Đề nghị nhận lớp từ phụ huynh " + customer.FullName, EmailSenderService.PopulateBodyChooseTutor(tutor, customer, item, "~/EmailTemplates/ChooseTutorForTeaching.html"));
                }
                catch (Exception ex)
                {
                    return Json(new { Status = "Fail" });
                }
            }

            
            return Json(new { Status = "OK"});
        }
        [AllowAnonymous]
         
        // GET: Tutors
        public ActionResult TutorRegistrationForm()
        {
            return View("TutorRegistrationForm", new PrivateTutorOnline.Models.ViewModels.TutorRegistrationFormViewModel() {
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList() });
        }
        [AllowAnonymous]
        public ActionResult ExistingTutorsList(int? page)
        {
            bool IsStillRemainClass = false;
            if (User.IsInRole("Customer"))
            {
                string UserId = User.Identity.GetUserId();
                IsStillRemainClass = db.RegistrationClasses.Include(t => t.Customer)
                    .Where(t => t.Customer.UserId == UserId && ( t.Status == ClassStatus.CustomerRejected || t.Status == ClassStatus.AdminApproved)).Count() > 0 ? true : false;
            }
            
            IList<Tutor> TutorList = db.Tutors.Include("Grades").Include("Subjects").Where(s => s.IsActivate && s.IsEnable).OrderByDescending(s => s.Id).ToList();
            return View("ExistingTutorList", new PrivateTutorOnline.Models.ViewModels.ExistingTutorListViewModel() {
                Tutors = TutorList.ToPagedList(page.HasValue ? page.Value : 1, 2),
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList(),
                IsStillClassRemained = IsStillRemainClass
            });
        }
       
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search(  string Keyword ,
         Gender Gender ,
         int Grade ,
         int Subject ,
         AcademicDegree Degree ,
         bool? IsAccurate , int? page)
        {
            SearchTutorBindingModel SearchData = new SearchTutorBindingModel();
            SearchData.IsAccurate = IsAccurate ?? false;
            SearchData.Subject = Subject;
            SearchData.Grade = Grade;
            SearchData.Degree = Degree;
            SearchData.Gender = Gender;
            SearchData.Keyword = Keyword;
            IList<Tutor> TutorList = null;
            if (SearchData.IsAccurate)
            {
                if(String.IsNullOrEmpty(SearchData.Keyword))
                {
                    TutorList = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects)
                   .Where(t =>
                       t.Grades.Any(g => g.Id == SearchData.Grade)
                       && t.Subjects.Any(s => s.Id == SearchData.Subject)
                       && t.Gender == SearchData.Gender
                       && t.Degree == SearchData.Degree
                   )
                   .ToList();
                }
                else
                {
                    TutorList = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects)
                   .Where(t =>
                        t.Grades.Any(g => g.Id == SearchData.Grade)
                       && t.Subjects.Any(s => s.Id == SearchData.Subject)
                       && t.Gender == SearchData.Gender
                       && t.Degree == SearchData.Degree
                       && ( t.Id == Int32.Parse(SearchData.Keyword)
                       || t.FullName.Contains(SearchData.Keyword)
                       || t.District.Contains(SearchData.Keyword)
                       || t.City.Contains(SearchData.Keyword)
                       || t.Street.Contains(SearchData.Keyword)
                       || t.Ward.Contains(SearchData.Keyword)
                       || t.MajorSubject.Contains(SearchData.Keyword)
                       || t.GraduationYear.Equals(SearchData.Keyword)
                       || t.PhoneNumber.Equals(SearchData.Keyword)
                       || t.HomeTown.Contains(SearchData.Keyword)
                       || t.University.Contains(SearchData.Keyword)
                       )
                   )
                   .ToList();
                }
                
            }
            else
            {
                if (String.IsNullOrEmpty(SearchData.Keyword))
                {
                    TutorList = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects)
                   .Where(t =>
                        t.Grades.Any(g => g.Id == SearchData.Grade)
                   || t.Subjects.Any(s => s.Id == SearchData.Subject)
                   || t.Gender == SearchData.Gender 
                   || t.Degree == SearchData.Degree
                   )
                   .ToList();
                }
                else
                {
                    TutorList = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects)
                   .Where(t =>
                        t.Grades.Any(g => g.Id == SearchData.Grade)
                   || t.Subjects.Any(s => s.Id == SearchData.Subject)
                   || t.Gender == SearchData.Gender
                   || t.Grades.Any(h => h.Name.Contains(SearchData.Keyword))
                   || t.Subjects.Any(h => h.Name.Contains(SearchData.Keyword))
                   || t.FullName.Contains(SearchData.Keyword)
                   || t.District.Contains(SearchData.Keyword)
                   || t.City.Contains(SearchData.Keyword)
                   || t.Street.Contains(SearchData.Keyword)
                   || t.Ward.Contains(SearchData.Keyword)
                   || t.MajorSubject.Contains(SearchData.Keyword)
                   || t.GraduationYear.Equals(SearchData.Keyword)
                   || t.PhoneNumber.Contains(SearchData.Keyword)
                   || t.HomeTown.Contains(SearchData.Keyword)
                   || t.University.Contains(SearchData.Keyword)
                   || t.Degree == SearchData.Degree
                   || t.Id == Int32.Parse(SearchData.Keyword)

                   )
                   .ToList();
                } 
            }
           
            return View("ExistingTutorList", new PrivateTutorOnline.Models.ViewModels.ExistingTutorListViewModel()
            {
                Tutors = TutorList.ToPagedList(page.HasValue ? page.Value : 1, 2),
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList(), 
                searchResult = SearchData
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchBySubject(int SubjectId, int? page)
        {
            IList<Tutor> TutorList = db.Tutors.Include("Grades").Include("Subjects")
                .Where(t =>t.Subjects.Any(s => s.Id == SubjectId))
                .ToList();
            return View("ExistingTutorList", new PrivateTutorOnline.Models.ViewModels.ExistingTutorListViewModel()
            {
                Tutors = TutorList.ToPagedList(page.HasValue ? page.Value : 1, 2),
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList()
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchByGrade(int GradeId, int? page)
        {
            IList<Tutor> TutorList = db.Tutors.Include("Grades").Include("Subjects")
                .Where(t => t.Grades.Any(s => s.Id == GradeId))
                .ToList();
            return View("ExistingTutorList", new PrivateTutorOnline.Models.ViewModels.ExistingTutorListViewModel()
            {
                Tutors = TutorList.ToPagedList(page.HasValue ? page.Value : 1, 2),
                Subjects = db.Subjects.ToList(),
                Grades = db.Grades.ToList()
            });
        }
        [ChildActionOnly]
        // GET: Tutors/Details/5
        public PartialViewResult TutorDetails(int Id)
        {
            Tutor tutor = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).SingleOrDefault( s=> s.Id == Id);
            TutorEditViewModel model = new TutorEditViewModel();
           
            model.FullName = tutor.FullName;
            model.City = tutor.City;
            model.District = tutor.District;
            model.Ward = tutor.Ward;
            model.Street = tutor.Street;
            model.Advantage = tutor.Advantage;
            model.DateOfBirth = tutor.DateOfBirth;
            model.Gender = tutor.Gender;
            model.Degree = tutor.Degree;
            model.Email = tutor.Email;
            model.GraduationYear = tutor.GraduationYear;
            model.HomeTown = tutor.HomeTown;
            model.IdentityNumber = tutor.IdentityNumber;
            model.MajorSubject = tutor.MajorSubject;
            model.PhoneNumber = tutor.PhoneNumber;
            model.UniversityName = tutor.University;
            model.Subjects = tutor.Subjects.ToList();
            model.Grades = tutor.Grades.ToList();
            model.SubjectsData = db.Subjects.ToList();
            model.GradesData = db.Grades.ToList();
            model.Avatar = tutor.Image;
            return PartialView(model);
        }

        [ChildActionOnly]
        // GET: Tutors/Details/5
        public PartialViewResult CustomerDetails(int Id)
        {
            Customer customer = db.Customers.Find(Id);

            return PartialView(customer);
        }

        // GET: Tutors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,HomeTown,Address,DateOfBirth,Email,PhoneNumber,Gender,IdentityNumber,University,MajorSubject,GraduationYear,Advantage,Degree,Image")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Tutors.Add(tutor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutor);
        }

        // GET: Tutors/Edit/5
        [HttpGet]
        public ActionResult EditTutorInfoView(int Id)
        {
            Tutor tutor = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).SingleOrDefault(s => s.Id == Id);
            TutorEditViewModel model = new TutorEditViewModel();

            model.FullName = tutor.FullName;
            model.City = tutor.City;
            model.District = tutor.District;
            model.Ward = tutor.Ward;
            model.Street = tutor.Street;
            model.Advantage = tutor.Advantage;
            model.DateOfBirth = tutor.DateOfBirth;
            model.Gender = tutor.Gender;
            model.Degree = tutor.Degree;
            model.Email = tutor.Email;
            model.GraduationYear = tutor.GraduationYear;
            model.HomeTown = tutor.HomeTown;
            model.IdentityNumber = tutor.IdentityNumber;
            model.MajorSubject = tutor.MajorSubject;
            model.PhoneNumber = tutor.PhoneNumber;
            model.UniversityName = tutor.University;
            model.Subjects = tutor.Subjects.ToList();
            model.Grades = tutor.Grades.ToList();
            model.SubjectsData = db.Subjects.ToList();
            model.GradesData = db.Grades.ToList();
            model.Avatar = tutor.Image; 
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTutorInfo(HttpPostedFileBase Avatar, PrivateTutorOnline.Models.BindingModels.TutorBindingModel tutorInfo)
        {
            Tutor tutor = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).SingleOrDefault(s => s.Id == tutorInfo.Id);
            tutor.Subjects.Clear();
            tutor.Grades.Clear();
            db.Entry(tutor).State = EntityState.Modified;
            db.SaveChanges();  
            tutor = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).SingleOrDefault(s => s.Id == tutorInfo.Id);
            if (tutor != null)
            {
                if (Avatar != null)
                { 
                    tutor.Image = new byte[Avatar.ContentLength];
                    Avatar.InputStream.Read(tutor.Image, 0, Avatar.ContentLength);
                }
                tutor.FullName = tutorInfo.FullName;
                tutor.City = tutorInfo.City;
                tutor.District = tutorInfo.District;
                tutor.Ward = tutorInfo.Ward;
                tutor.Street = tutorInfo.Street;
                tutor.Advantage = tutorInfo.Advantage;
                if(tutorInfo.DateOfBirth.HasValue)
                {
                    tutor.DateOfBirth = tutorInfo.DateOfBirth.Value;
                }
                tutor.Gender = tutorInfo.Gender;
                tutor.Degree = tutorInfo.Degree;
                tutor.Email = tutorInfo.Email;
                tutor.GraduationYear = tutorInfo.GraduationYear;
                tutor.HomeTown = tutorInfo.HomeTown;
                tutor.IdentityNumber = tutorInfo.IdentityNumber;
                tutor.MajorSubject = tutorInfo.MajorSubject;
                tutor.PhoneNumber = tutorInfo.PhoneNumber;
                tutor.University = tutorInfo.UniversityName;
                if(tutorInfo.Subjects.Count > 0)
                {
                    tutor.Subjects = new List<Subject>();
                    foreach (int i in tutorInfo.Subjects)
                    {

                        tutor.Subjects.Add(db.Subjects.SingleOrDefault(s => s.Id == i));
                    }
                }
                if (tutorInfo.Grades.Count > 0)
                {
                    tutor.Grades = new List<Grade>(); 
                    foreach (int i in tutorInfo.Grades)
                    {

                        tutor.Grades.Add(db.Grades.SingleOrDefault(gr => gr.Id == i));
                    }
                }
                    
                UserManager.SetEmail(User.Identity.GetUserId(), tutor.Email);
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Manage");
            }
            else
                return View("Error"); 
        }
        [HttpGet]
        public ActionResult EditCustomerInfoView(int Id)
        { 
            return View(db.Customers.SingleOrDefault(s => s.Id == Id));
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomerInfo(Customer customer)
        { 
            customer.UserId = User.Identity.GetUserId(); 
            UserManager.SetEmail(User.Identity.GetUserId(), customer.Email); 
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
                
            return RedirectToAction("Index", "Manage", new { message = "Bạn đã thay đổi thông tin hồ sơ thành công." });
             
        }

        // GET: Tutors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutor tutor = db.Tutors.Find(id);
            db.Tutors.Remove(tutor);
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
