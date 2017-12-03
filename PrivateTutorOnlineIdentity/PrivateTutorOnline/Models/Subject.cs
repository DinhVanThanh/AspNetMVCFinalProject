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
        public int Id { get; set; }
        public string Name { get; set; }
        [InverseProperty("Subjects")]
        public ICollection<Tutor> Tutors { get; set; }
        [InverseProperty("Subjects")]
        public ICollection<RegistrationClass> RegistrationClass { get; set; }
    }
}