using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Subject
    {
        [Key]
        [Display(Name = "Mã số")]
        public int Id { get; set; }
        [Display(Name = "Tên môn học")]
        public string Name { get; set; }
        [Display(Name = "Gia sư")]
        [InverseProperty("Subjects")]
        public ICollection<Tutor> Tutors { get; set; }
        [InverseProperty("Subjects")]
        public ICollection<RegistrationClass> RegistrationClass { get; set; }
    }
}