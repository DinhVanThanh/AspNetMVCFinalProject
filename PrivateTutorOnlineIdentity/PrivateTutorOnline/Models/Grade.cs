using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Grade
    {
        [Key]
        [Display(Name = "Mã số")]
        public int Id { get; set; }
        [Display(Name = "Tên lớp")]
        public string Name { get; set; }
        [Display(Name = "Tên gia sư")]
        public ICollection<Tutor> Tutors { get; set; }
    }
}