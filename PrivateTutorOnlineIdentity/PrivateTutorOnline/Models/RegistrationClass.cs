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
        [Display(Name = "Mã số")]
        public int Id { get; set; }
        [Display(Name = "Lớp")]
        public Grade Grade { get; set; }
        [Display(Name = "Môn học")]
        public IList<Subject> Subjects { get; set; }
        [Display(Name = "Học phí / tháng")]
        [StringLength(20)]
        public string SalaryPerMonth { get; set; }
        [Display(Name = "Số buổi / tuần")]
        public short? DayPerWeek { get; set; }
        [Display(Name = "Lịch dạy / tuần")]
        [StringLength(200)]
        public string TutoringTime { get; set; }
        [Display(Name = "Yêu cầu")]
        [StringLength(200)]
        public string Requirement { get; set; }
        [Display(Name = "Ngày nhận")]
        public DateTime? ReceivedDate { get; set; }
        [Display(Name = "Trạng thái")]
        public ClassStatus? Status { get; set; }
        [Display(Name = "Thành phố")]
        [StringLength(50)]
        public string City { get; set; }
        [Display(Name = "Quận")]
        [StringLength(50)]
        public string District { get; set; }
        [Display(Name = "Phường")]
        [StringLength(50)]
        public string Ward { get; set; }
        [Display(Name = "Đường")]
        [StringLength(100)]
        public string Street { get; set; }
        [Display(Name = "Phụ huynh")]

        public Customer Customer { get; set; }
        [Display(Name = "Gia sư")]
        public Tutor Tutor { get; set; }
        [Display(Name = "Is Active")]
        public bool IsClosed { get; set; }
    }
}