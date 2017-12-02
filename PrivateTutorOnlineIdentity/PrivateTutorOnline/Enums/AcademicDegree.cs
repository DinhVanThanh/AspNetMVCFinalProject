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
        Student = 0,
        [Display(Name = "Giáo viên")]
        Teacher,
        [Display(Name = "Giảng viên")]
        Lecturer,
        [Display(Name = "Kĩ sư")]
        Engineer,
        [Display(Name = "Cử nhân")]
        Bachelor,
        [Display(Name = "Thạc sĩ")]
        Master,
        [Display(Name = "Tiến sĩ")]
        Doctor,
        [Display(Name = "Khác")]
        Other
    }
}