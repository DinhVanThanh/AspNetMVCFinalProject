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
            return PartialView("_SideBar", new SidebarViewModel() { Subjects = Context.Subjects.ToList(), Grades = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 } });
        }
         
    }
}
