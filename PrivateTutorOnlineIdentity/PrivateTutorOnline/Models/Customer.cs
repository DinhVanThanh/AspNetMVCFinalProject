﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Customer 
    {
        [Key]
        [Display(Name = "Mã số")]
        public int Id { get; set; } 
        public string UserId { get; set; }
        [Display(Name = "Họ Tên")]
        [StringLength(100)]
        public string FullName { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        
        public string Email { get; set; }

        [Display(Name = "Thành phố")]
        [StringLength(50)]
        public string City { get; set; }
        [Display(Name = "Quận")]
        [StringLength(50)]
        public string District { get; set; }
        [Display(Name = "Phường")]
        [StringLength(100)]
        public string Ward { get; set; }
        [Display(Name = "Tên đường")]
        [StringLength(100)]
        public string Street { get; set; }
        [Display(Name = "Kích hoạt")] 
        public bool IsActivate { get; set; }
        [Display(Name = "Trạng thái")] 
        public bool IsEnable { get; set; }

        public IList<RegistrationClass> RegistrationClasss { get; set; }
    }
}