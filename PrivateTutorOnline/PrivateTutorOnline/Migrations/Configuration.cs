namespace PrivateTutorOnline.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PrivateTutorOnline.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PrivateTutorOnline.Models.TutorOnlineDBContext>
    {
        public Configuration()
        {

            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }
        public bool AddUserToRole(UserManager<ApplicationUser> _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole<ApplicationUser, string>(userId, roleName);
            return idResult.Succeeded;
        }

        protected override void Seed(PrivateTutorOnline.Models.TutorOnlineDBContext context)
        {
            bool success = false;

            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = this.CreateRole(_roleManager, "Admin", "Global Access");
            

            success = this.CreateRole(_roleManager, "Owner", "Managing existing records");
            

            success = this.CreateRole(_roleManager, "Customer", "Restricted to business domain activity");
            

            // Create my debug (testing) objects here

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ApplicationUser user = new ApplicationUser();
            PasswordHasher passwordHasher = new PasswordHasher();
             
            user.UserName = "dinhvanthanh";
            user.Email = "thanh.kyanon@gmail.com"; 
            user.LockoutEnabled = true;

            IdentityResult result = userManager.Create(user, "123456");
            if(result.Succeeded)
            success = this.AddUserToRole(userManager, user.Id, "Admin");



            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if ((context.Subjects.Count() == 0))
            {
                context.Subjects.AddOrUpdate(
               new Models.Subject() { Name = "Toán" },
               new Models.Subject() { Name = "Tiếng Anh" },
               new Models.Subject() { Name = "Lý" },
               new Models.Subject() { Name = "Hóa" },
               new Models.Subject() { Name = "Sinh" },
               new Models.Subject() { Name = "Sử" },
               new Models.Subject() { Name = "Địa Lý" },
               new Models.Subject() { Name = "Vẽ" },
               new Models.Subject() { Name = "Vi Tính" },
               new Models.Subject() { Name = "Đàn-Nhạc" }
               );
            }
            if(context.Grades.Count() == 0)
            {
                context.Grades.AddOrUpdate(
                new Models.Grade() { Name = "1" },
                new Models.Grade() { Name = "2" },
                new Models.Grade() { Name = "3" },
                new Models.Grade() { Name = "4" },
                new Models.Grade() { Name = "5" },
                new Models.Grade() { Name = "6" },
                new Models.Grade() { Name = "7" },
                new Models.Grade() { Name = "8" },
                new Models.Grade() { Name = "9" },
                new Models.Grade() { Name = "10" },
                new Models.Grade() { Name = "11" },
                new Models.Grade() { Name = "12" },
                new Models.Grade() { Name = "Luyện thi đại học" }
                );
            }

            if (context.Customers.Count() == 0)
            {
                context.Customers.AddOrUpdate(
                   new Models.Customer()
                   {
                       FullName = "Nguyễn Hồng Hạnh",
                       PhoneNumber = "01213546546",
                       Email = "Hong.Hanh@gmail.com",
                       City = "TPHCM",
                       District = "Quận 5",
                       Ward = "Phường 13",
                       Street = "An Dương Vương"
                   }
                  );
            }
            
            if (context.Tutors.Count() == 0)
            {
                context.Tutors.AddOrUpdate(
                new Models.Tutor()
                {
                    FullName = "Hoàng Tuấn Anh",
                    Gender = Enums.Gender.Male,
                    DateOfBirth = new DateTime(1994, 11, 2),
                    Email = "Tuan.Anh@gmail.com",
                    PhoneNumber = "01526487656",
                    IdentityNumber = "0225644478",
                    HomeTown = "Tỉnh Hà Nam",
                    Address = "12 Tôn Đức Thắng, Quận 10, TPHCM",
                    University = "ĐH Sư Phạm TPHCM",
                    MajorSubject = "Sư Phạm Toán Học",
                    GraduationYear = "2016",
                    Advantage = "Đã từng đi dạy",
                    Degree = Enums.AcademicDegree.Teacher,
                    Image = new byte[] { }
                },
                new Models.Tutor()
                {
                    FullName = "Nguyễn Ngọc Ánh",
                    Gender = Enums.Gender.Female,
                    DateOfBirth = new DateTime(1993, 2, 2),
                    Email = "Ngoc.Anh@gmail.com",
                    PhoneNumber = "01526487656",
                    IdentityNumber = "0225644478",
                    HomeTown = "TP Đà Nẵng",
                    Address = "12 Hai Bà Trưng, Quận 3, TPHCM",
                    University = "ĐH Ngoại Thương TPHCM",
                    MajorSubject = "Quản trị kinh doanh",
                    GraduationYear = "2015",
                    Advantage = "Đã từng đi dạy",
                    Degree = Enums.AcademicDegree.Master,
                    Image = new byte[] { }
                },
                new Models.Tutor()
                {
                    FullName = "Vương Tuấn Kiệt",
                    Gender = Enums.Gender.Male,
                    DateOfBirth = new DateTime(1995, 11, 11),
                    Email = "Tuan.Kiet@gmail.com",
                    PhoneNumber = "01526487656",
                    IdentityNumber = "0225644478",
                    HomeTown = "Tỉnh Đồng Tháp",
                    Address = "12/2C Bùi Thị Xuân , Quận 1 , TPHCM",
                    University = "Cao Đẳng Kinh Tế Đối Ngoại",
                    MajorSubject = "Kế toán",
                    GraduationYear = "2017",
                    Advantage = "Đã từng đi dạy",
                    Degree = Enums.AcademicDegree.Student,
                    Image = new byte[] { }
                },
                 new Models.Tutor()
                 {
                     FullName = "Đỗ Thị Phương Nhung",
                     Gender = Enums.Gender.Female,
                     DateOfBirth = new DateTime(1996, 5, 13),
                     Email = "Phuong.Nhung@gmail.com",
                     PhoneNumber = "01526487656",
                     IdentityNumber = "0225644478",
                     HomeTown = "Thủ đô Hà Nội",
                     Address = "12/2C Nguyễn Kim , Quận 5 , TPHCM",
                     University = "Đại Học Sài Gòn",
                     MajorSubject = "Sư Phạm Tiếng Anh",
                     GraduationYear = "2017",
                     Advantage = "Đã từng đi dạy",
                     Degree = Enums.AcademicDegree.Bachelor,
                     Image = new byte[] { }
                 }
                );
            }
            


        }
    }
}
