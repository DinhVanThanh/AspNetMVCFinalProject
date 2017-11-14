using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.BindingModels
{
    public class SearchTutorBindingModel
    {
        public string Keyword { get; set; }
        public Gender Gender { get; set; }
        public int Grade { get; set; }
        public int Subject { get; set; }
    }
}