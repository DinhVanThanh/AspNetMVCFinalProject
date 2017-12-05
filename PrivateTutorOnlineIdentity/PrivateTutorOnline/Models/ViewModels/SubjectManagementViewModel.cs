using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class SubjectManagementViewModel
    {
        public IPagedList<Subject> Subjects { get; set; }
        public string searchString { get; set; }
        public bool IsSearchById { get; set; }
    }
}