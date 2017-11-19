using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Customer 
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; } 
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUserRole Role { get; set; }
        public IList<RegistrationClass> RegistrationClasss { get; set; }
    }
}