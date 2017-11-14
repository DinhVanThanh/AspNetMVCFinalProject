using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class ClassRegistrationFormViewModel
    {
        public IList<Grade> Grades { get; set; }
        public IList<Subject> Subjects { get; set; }
    }
}