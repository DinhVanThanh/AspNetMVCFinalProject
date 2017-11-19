using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.BindingModels
{
    public class ClassRegistrationBindingModel
    {
         
        public Grade Grade { get; set; }
        public IList<int> Subjects { get; set; } 
        public short LearnerNumber { get; set; }
        public string TeachingTime { get; set; }
        public string SalaryPerMonth { get; set; }
        public short SessionPerWeek { get; set; } 
        public string Requirement { get; set; }
        public DateTime ReceivedDate { get; set; } 
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
    }
}