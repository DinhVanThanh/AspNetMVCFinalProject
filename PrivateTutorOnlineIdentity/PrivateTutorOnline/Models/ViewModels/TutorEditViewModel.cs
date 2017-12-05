using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class TutorEditViewModel
    {
        [Display(Name = "Mã số")]
        public int Id { get; set; }
        [Display(Name = "Họ Tên")]
        public string FullName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Thành phố")]
        public string City { get; set; }
        [Display(Name = "Quận")]
        public string District { get; set; }
        [Display(Name = "Phường")]
        public string Ward { get; set; }
        [Display(Name = "Đường")]
        public string Street { get; set; }
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }
        [Display(Name = "Số CMND")]
        public string IdentityNumber { get; set; }
        [Display(Name = "Nguyên quán")]
        public string HomeTown { get; set; }
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]  // format used by Html.EditorFo
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Trình độ")]
        public AcademicDegree Degree { get; set; }
        [Display(Name = "Trường")]
        public string UniversityName { get; set; }
        [Display(Name = "Chuyên ngành")]
        public string MajorSubject { get; set; }
        [Display(Name = "Năm tốt nghiệp")]
        public string GraduationYear { get; set; }
        [Display(Name = "Ưu điểm")]
        public string Advantage { get; set; }
        [Display(Name = "Lớp")]
        public IList<Grade> Grades { get; set; }
        [Display(Name = "Môn học")]
        public IList<Subject> Subjects { get; set; }
        public IList<Subject> SubjectsData { get; set; }
        public IList<Grade> GradesData { get; set; }
        [Display(Name = "Hình")]
        public byte[] Avatar { get; set; }
    }
}