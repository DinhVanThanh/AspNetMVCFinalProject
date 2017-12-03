using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateTutorOnline.Models;
using PrivateTutorOnline.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrivateTutorOnline.Services;
using PagedList;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles = "Admin, Customer, Tutor")]
    public class ManageRegistrationClassesController : Controller
    {
        private TutorOnlineDBContext db = new TutorOnlineDBContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private string AdminEmail = "tieuluantotnghiep2017@gmail.com";
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
        // GET: ManageRegistrationClasses
        public async Task<ActionResult> Index()
        {
            return View(await db.RegistrationClasses.ToListAsync());
        }
        public async Task<ActionResult> EnrolledClass(string searchString, int? page)
        {
            string UserId = User.Identity.GetUserId();
            IList<EnrolledClassViewModel> enrolledClass = new List<EnrolledClassViewModel>();
            foreach (var item in db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Where(s => s.Tutor.UserId == UserId).ToList())
            {
                enrolledClass.Add(new EnrolledClassViewModel()
                {
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    ReceivedDate = item.ReceivedDate
                });
            }
            return View(enrolledClass.ToPagedList(page ?? 1, 2).OrderByDescending(s => s.Id)); 
        }
        public async Task<ActionResult> PostedClass(string searchString, int? page)
        {
            string UserId = User.Identity.GetUserId();
            IList<PostedClassViewModel> postedClass = new List<PostedClassViewModel>();
            foreach(var item in db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Where(s => s.Customer.UserId == UserId).Include(s => s.Tutor).ToList())
            {
                postedClass.Add(new PostedClassViewModel() {
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    Tutor = item.Tutor,
                    ReceivedDate = item.ReceivedDate
                });
            }
            return View(postedClass.ToPagedList(page ?? 1, 2).OrderByDescending(s => s.Id));
        }
        public async Task<ActionResult> AllPostedClass(string searchString, int? page)
        { 
            IList<PostedClassViewModel> allPostedClass = new List<PostedClassViewModel>();
            foreach (var item in db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).ToList())
            {
                allPostedClass.Add(new PostedClassViewModel()
                {
                    Id = item.Id,
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    ReceivedDate = item.ReceivedDate
                });
            }
            return View(allPostedClass.ToPagedList(page ?? 1, 2).OrderByDescending(s => s.Id));
        }
        [HttpPost]
        public HttpStatusCodeResult ReceiveClass(int ClassId)
        {
            try
            {
                string tutorUsername = User.Identity.GetUserName();
               
                string UserId = User.Identity.GetUserId();
                
                RegistrationClass RegistrationClass = db.RegistrationClasses.Include(s => s.Customer).SingleOrDefault(s => s.Id == ClassId);
                Customer customer = RegistrationClass.Customer;
                string customerUsername = UserManager.FindById(customer.UserId).UserName;
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.UserId == UserId);
                RegistrationClass.Tutor = tutor;
                RegistrationClass.Status = Enums.ClassStatus.TutorRegistered;
                RegistrationClass.ReceivedDate = DateTime.Now;
                db.Entry(RegistrationClass).State = EntityState.Modified;
                db.SaveChanges();
                //send to tutor
                EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Nhận lớp mã số : " + RegistrationClass.Id,
                            EmailSenderService.PopulateBody(customer.FullName, tutor.FullName, RegistrationClass.Id.ToString() , "~/EmailTemplates/ClassTutorEnrollmentNotificationToTutor.html"));
                //send to customer
                EmailSenderService.SendHtmlFormattedEmail(customer.Email, "Gia sư nhận lớp",
                            EmailSenderService.PopulateBodyTutorEnrollClassNotificationToCustomer(customer.FullName, tutor, RegistrationClass.Id.ToString(), "~/EmailTemplates/ClassTutorEnrollmentNotificationToCustomer.html"));
                //send to admin
                EmailSenderService.SendHtmlFormattedEmail(AdminEmail, "Có gia sư nhận lớp",
                            EmailSenderService.PopulateBodyTutorEnrollClassNotificationToAdmin(customer, customerUsername, tutor, tutorUsername, RegistrationClass.Id.ToString(), "~/EmailTemplates/ClassTutorEnrollmentNotificationToAdmin.html"));
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: ManageRegistrationClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageRegistrationClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SalaryPerMonth,DayPerWeek,TutoringTime,Requirement,ReceivedDate,Status,City,District,Ward,Street,IsActive")] RegistrationClass registrationClass)
        {
            if (ModelState.IsValid)
            {
                db.RegistrationClasses.Add(registrationClass);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // POST: ManageRegistrationClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SalaryPerMonth,DayPerWeek,TutoringTime,Requirement,ReceivedDate,Status,City,District,Ward,Street,IsActive")] RegistrationClass registrationClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // POST: ManageRegistrationClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            db.RegistrationClasses.Remove(registrationClass);
            await db.SaveChangesAsync();
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
