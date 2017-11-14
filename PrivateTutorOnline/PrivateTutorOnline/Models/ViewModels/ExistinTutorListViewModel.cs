using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class ExistingTutorListViewModel
    {
        public IList<Tutor> Tutors { get; set; }
        public IList<Subject> Subjects { get; set; }
        public IList<Grade> Grades { get; set; } 
    }
}