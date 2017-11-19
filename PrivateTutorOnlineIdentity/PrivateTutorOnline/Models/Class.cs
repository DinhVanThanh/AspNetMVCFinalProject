using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public Tutor Tutor { get; set; }
        public Customer Customer { get; set; }
        public Subject Subject { get; set; }
        public int DayPerWeek { get; set; }
    }
}