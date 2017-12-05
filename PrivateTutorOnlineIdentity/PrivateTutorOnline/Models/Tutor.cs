using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class Tutor 
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
        [EmailAddress]
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
        [Display(Name = "Nguyên Quán")]
        public string HomeTown { get; set; } 
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }  
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }
        [Display(Name = "Số CMND")]
        public string IdentityNumber { get; set; }
        [Display(Name = "Trường")]
        public string University { get; set; }
        [Display(Name = "Chuyên ngành")]
        public string MajorSubject { get; set; }
        [Display(Name = "Năm tốt nghiệp")]
        public string GraduationYear { get; set; }
        [Display(Name = "Ưu điểm")]
        public string Advantage { get; set; }
        [Display(Name = "Trình độ")]
        public AcademicDegree Degree { get; set; }
        [Display(Name = "Hình")]
        public byte[] Image { get; set; }
        [Display(Name ="Môn dạy")]
        public ICollection<Subject> Subjects { get; set; } 
        [Display(Name = "Lớp dạy")]
        public ICollection<Grade> Grades { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActivate { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsEnable { get; set; }
    }
}