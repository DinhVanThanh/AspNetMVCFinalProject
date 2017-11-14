using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Enums
{
    public enum AcademicDegree
    {
        [Display(Name = "Sinh viên")]
        Student,
        [Display(Name = "Sinh viên")]
        Teacher,
        [Display(Name = "Sinh viên")]
        Lecturer,
        [Display(Name = "Sinh viên")]
        Engineer,
        [Display(Name = "Sinh viên")]
        Bachelor,
        [Display(Name = "Sinh viên")]
        Master,
        Other
    }
}