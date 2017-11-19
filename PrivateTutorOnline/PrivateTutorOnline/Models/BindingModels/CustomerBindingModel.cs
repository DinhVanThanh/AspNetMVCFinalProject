using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.BindingModels
{
    public class CustomerBindingModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
    }
}