using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class RegistrationClass
    {
        [Key]
        public int Id { get; set; }
        public Grade Grade { get; set; }
        public IList<Subject> Subjects { get; set; } 
        [StringLength(10)]
        public string SalaryPerMonth { get; set; } 
        public short DayPerWeek { get; set; }
        [StringLength(200)]
        public string TutoringTime { get; set; }
        [StringLength(200)]
        public string Requirement { get; set; }
        public DateTime ReceivedDate { get; set; }
        public ClassStatus Status { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(50)]
        public string Ward { get; set; }
        [StringLength(100)]
        public string Street { get; set; }

        public Customer Customer { get; set; }
        public Tutor Tutor { get; set; }
        public bool IsActive { get; set; }
    }
}