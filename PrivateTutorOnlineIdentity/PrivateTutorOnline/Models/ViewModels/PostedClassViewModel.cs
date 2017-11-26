using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models.ViewModels
{
    public class PostedClassViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Lớp")]
        public Grade Grade { get; set; }
        [Display(Name = "Môn học")]
        public IList<Subject> Subjects { get; set; }
        [Display(Name = "Học phí")]
        public string SalaryPerMonth { get; set; }
        [Display(Name = "Số ngày / tuần")]
        public short DayPerWeek { get; set; }
        [Display(Name = "Thời gian dạy / tuần")]
        public string TutoringTime { get; set; }
        [Display(Name = "Yêu cầu")]
        public string Requirement { get; set; }
        [Display(Name = "Ngày nhận lớp")]
        public DateTime ReceivedDate { get; set; }
        [Display(Name = "Trạng thái")]
        public ClassStatus Status { get; set; }
        [Display(Name = "Thành phố")]
        public string City { get; set; }
        [Display(Name = "Quận")]
        public string District { get; set; }
        [Display(Name = "Phường")]
        public string Ward { get; set; }
        [Display(Name = "Đường")]
        public string Street { get; set; }
        [Display(Name = "Tên gia sư")]
        public Tutor Tutor { get; set; }
    }
}