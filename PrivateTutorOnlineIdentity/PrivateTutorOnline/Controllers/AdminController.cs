using PrivateTutorOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        TutorOnlineDBContext db = new TutorOnlineDBContext();
        // GET: Admin
        public ActionResult CustomerManagementView()
        {
            IList<Customer> Customers = db.Customers.ToList();
            return View(Customers);
        }
        public JsonResult ActivateCustomer(int CustomerId)
        {
            try
            {
                Customer customer = db.Customers.SingleOrDefault(s => s.Id == CustomerId);
                if (!customer.IsActivate)
                    customer.IsActivate = true;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            
            return Json(new { Status = "OK" });
        }
        public JsonResult DisableCustomer(int CustomerId)
        {
            try
            {
                Customer customer = db.Customers.SingleOrDefault(s => s.Id == CustomerId);
                if (!customer.IsEnable)
                    customer.IsEnable = true;
                else
                    customer.IsEnable = false;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            
            return Json(new { Status = "OK" });
        }
        public ActionResult TutorManagementView()
        {
            IList<Tutor> Tutors = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).ToList();
            return View(Tutors);
        }
        public JsonResult ActivateTutor(int TutorId)
        {
            try
            {
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.Id == TutorId);
                if (!tutor.IsActivate)
                    tutor.IsActivate = true;
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "OK" });
        }
        public JsonResult DisableTutor(int TutorId)
        {
            try
            {
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.Id == TutorId);
                if (!tutor.IsEnable)
                    tutor.IsEnable = true;
                else
                    tutor.IsEnable = false;
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            } 
            
            return Json(new { Status = "OK" });
        }
        public ActionResult ClassManagementView()
        {
            IList<RegistrationClass> Classes = db.RegistrationClasses.Include(s => s.Tutor).Include(s => s.Customer).ToList();
            return View(Classes);
        }
        [HttpPost]
        public JsonResult ApproveOrRejectClass(int ClassId, bool IsApproved)
        {
            try
            {
                RegistrationClass Class = db.RegistrationClasses.SingleOrDefault(s => s.Id == ClassId);
                if (IsApproved)
                    Class.Status = Enums.ClassStatus.AdminApproved;
                else
                    Class.Status = Enums.ClassStatus.Reject;
                db.Entry(Class).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "OK" });
        }
        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
