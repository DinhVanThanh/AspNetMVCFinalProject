using PrivateTutorOnline.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorOnline.Controllers
{
    public class BaseController : Controller
    {
        private PrivateTutorOnline.Models.TutorOnlineDBContext Context = new Models.TutorOnlineDBContext(); 
        [ChildActionOnly]
        // GET: Base
        public PartialViewResult SideBar()
        {
            return PartialView("_SideBar", new SidebarViewModel() { Subjects = Context.Subjects.ToList(), Grades = Context.Grades.ToList() });
        }
       
    }
}
