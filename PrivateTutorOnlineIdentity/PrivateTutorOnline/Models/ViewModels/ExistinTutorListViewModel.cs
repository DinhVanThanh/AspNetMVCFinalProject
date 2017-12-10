using PagedList;
using PrivateTutorOnline.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class ExistingTutorListViewModel
    {
        public IPagedList<Tutor> Tutors { get; set; }
        public IList<Subject> Subjects { get; set; }
        public IList<Grade> Grades { get; set; } 
        public bool IsStillClassRemained { get; set; }
        public SearchTutorBindingModel searchResult { get; set; }
    }
}