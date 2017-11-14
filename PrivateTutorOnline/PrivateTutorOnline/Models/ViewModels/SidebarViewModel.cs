using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class SidebarViewModel
    {
        public IList<Subject> Subjects { get; set; }
        public IList<int> Grades { get; set; }
    }
}