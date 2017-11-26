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

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles = "Admin, Customer, Tutor")]
    public class ManageRegistrationClassesController : Controller
    {
        private TutorOnlineDBContext db = new TutorOnlineDBContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
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
        public async Task<ActionResult> EnrolledClass()
        {
            string UserId = User.Identity.GetUserId();
            IList<EnrolledClassViewModel> enrolledClass = new List<EnrolledClassViewModel>();
            foreach (var item in db.RegistrationClasses.Where(s => s.Tutor.UserId == UserId).ToList())
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
                    Street = item.Street
                });
            }
            return View(enrolledClass); 
        }
        public async Task<ActionResult> PostedClass()
        {
            string UserId = User.Identity.GetUserId();
            IList<PostedClassViewModel> postedClass = new List<PostedClassViewModel>();
            foreach(var item in db.RegistrationClasses.Where(s => s.Customer.UserId == UserId).Include(s => s.Tutor).ToList())
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
                    Tutor = item.Tutor
                });
            }
            return View(postedClass);
        }
        public async Task<ActionResult> AllPostedClass()
        { 
            IList<PostedClassViewModel> allPostedClass = new List<PostedClassViewModel>();
            foreach (var item in db.RegistrationClasses.ToList())
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
                    Street = item.Street
                });
            }
            return View(allPostedClass);
        }
        [HttpPost]
        public HttpStatusCodeResult ReceiveClass(int ClassId)
        {
            try
            {
                string UserId = User.Identity.GetUserId();
                RegistrationClass RegistrationClass = db.RegistrationClasses.SingleOrDefault(s => s.Id == ClassId);
                RegistrationClass.Tutor = db.Tutors.SingleOrDefault(s => s.UserId == UserId);
                RegistrationClass.Status = Enums.ClassStatus.TutorRegistered;
                db.Entry(RegistrationClass).State = EntityState.Modified;
                db.SaveChanges();
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
