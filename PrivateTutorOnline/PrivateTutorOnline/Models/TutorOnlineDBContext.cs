namespace PrivateTutorOnline.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TutorOnlineDBContext : IdentityDbContext<ApplicationUser>
    {
        // Your context has been configured to use a 'TutorOnlineDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PrivateTutorOnline.Models.TutorOnlineDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TutorOnlineDBContext' 
        // connection string in the application configuration file.
        public TutorOnlineDBContext()
            : base("name=TutorOnlineDBContext")
        {
            Database.SetInitializer<TutorOnlineDBContext>(new CreateDatabaseIfNotExists<TutorOnlineDBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TutorOnlineDBContext, PrivateTutorOnline.Migrations.Configuration>("TutorOnlineDBContext"));
        }

        
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Tutor> Tutors { set; get; }
        public virtual DbSet<Class> Classes { set; get; }
        public virtual DbSet<RegistrationClass> RegistrationClasses { set; get; }
        public virtual DbSet<Customer> Customers { set; get; }
        public virtual DbSet<Grade> Grades { set; get; } 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            //Defining the keys and relations 
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.Roles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        }


    } 
}