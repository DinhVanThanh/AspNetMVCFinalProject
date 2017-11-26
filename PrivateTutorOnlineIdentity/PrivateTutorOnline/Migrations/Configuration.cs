namespace PrivateTutorOnline.Migrations
{
    using System;
    using System.Globalization; 
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web; 
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using PrivateTutorOnline.Models;
    using System.Data.Entity.Migrations;
    using System.Linq; 
    using System.Web.Mvc; 

    internal sealed class Configuration : DbMigrationsConfiguration<PrivateTutorOnline.Models.TutorOnlineDBContext>
    {
        private ApplicationRoleManager _AppRoleManager;
        private ApplicationUserManager _userManager;
        protected ApplicationRoleManager AppRoleManager
        {
            get
            { 
                return _AppRoleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }
        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PrivateTutorOnline.Models.TutorOnlineDBContext context)
        {
            const string name = "Admin";
            const string password = "123456";
            const string roleName = "Admin";
            if(AppRoleManager.FindByNameAsync("Admin") != null)
                AppRoleManager.CreateAsync(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));
            if (AppRoleManager.FindByNameAsync("Customer") != null)
                AppRoleManager.CreateAsync(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Customer"));
            if (AppRoleManager.FindByNameAsync("Tutor") != null)
                AppRoleManager.CreateAsync(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Tutor"));
            if (AppRoleManager.FindByNameAsync("Owner") != null)
                AppRoleManager.CreateAsync(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Owner"));

            var admin = UserManager.FindByName(name);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = name, Email = "thanh.kyanon@gmail.com" };
                IdentityResult AdminCreationResult = UserManager.Create(admin, password);
                AdminCreationResult = UserManager.SetLockoutEnabled(admin.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = UserManager.GetRoles(admin.Id);
            if (!rolesForUser.Contains(roleName))
            {
                Task<IdentityResult> AdminRoleAddition = UserManager.AddToRoleAsync(admin.Id, roleName);
            }
            //Initializer Tutor account
            ApplicationUser customerUser = new ApplicationUser() { UserName = "dinhvanthanh1995@gmail.com", Email = "dinhvanthanh1995@gmail.com" };
            var CustomerCreationResult = UserManager.CreateAsync(customerUser, password);
            if (CustomerCreationResult.IsCompleted)
                CustomerCreationResult = UserManager.SetLockoutEnabledAsync(customerUser.Id, false);
            if (CustomerCreationResult.IsCompleted)
                UserManager.AddToRoleAsync(customerUser.Id, "Customer");
            var tutor = new ApplicationUser { UserName = "tieuluantotnghiep2017@gmail.com", Email = "tieuluantotnghiep2017@gmail.com" };
            var result = UserManager.CreateAsync(tutor, password);
            if (result.IsCompleted)
                result = UserManager.SetLockoutEnabledAsync(tutor.Id, false);
            if (result.IsCompleted)
                UserManager.AddToRoleAsync(tutor.Id, "Tutor");
           //Initializer Customer account
            
            

            
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
            if (context.Grades.Count() == 0)
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

            if (context.Customers.SingleOrDefault(c => c.Email == "dinhvanthanh1995@gmail.com") == null)
            {
               
                context.Customers.AddOrUpdate(
                   new Models.Customer()
                   {
                       FullName = "Đinh Văn Thành",
                       PhoneNumber = "01213546546",
                       Email = "dinhvanthanh1995@gmail.com",
                       City = "TPHCM",
                       District = "Quận 5",
                       Ward = "Phường 13",
                       Street = "An Dương Vương"
                   }
                );
                
            }

            if(context.Tutors.SingleOrDefault(s => s.Email == "tieuluantotnghiep2017@gmail.com") == null &&
                context.Tutors.SingleOrDefault(s => s.Email == "Tuan.Kiet@gmail.com") == null &&
                context.Tutors.SingleOrDefault(s => s.Email == "Ngoc.Anh@gmail.com") == null &&
                context.Tutors.SingleOrDefault(s => s.Email == "Phuong.Nhung@gmail.com") == null 
                )
            {
                
                context.Tutors.AddOrUpdate(
                new Models.Tutor()
                {
                    FullName = "Hoàng Tuấn Anh",
                    Gender = Enums.Gender.Male,
                    DateOfBirth = new DateTime(1994, 11, 2),
                    Email = "tieuluantotnghiep2017@gmail.com",
                    PhoneNumber = "01526487656",
                    IdentityNumber = "0225644478",
                    HomeTown = "Tỉnh Hà Nam",
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
