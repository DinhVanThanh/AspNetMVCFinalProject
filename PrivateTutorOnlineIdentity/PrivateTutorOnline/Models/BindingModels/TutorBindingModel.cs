using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.BindingModels
{
    public class TutorBindingModel
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public string IdentityNumber { get; set; }
        public string HomeTown { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AcademicDegree Degree { get; set; }
        public string UniversityName { get; set; }
        public string MajorSubject { get; set; }
        public string GraduationYear { get; set; }
        public string Advantage { get; set; } 
        public IList<int> Grades { get; set; }
        public IList<int> Subjects { get; set; }
    }
}