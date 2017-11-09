namespace PrivateTutorOnline.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TutorOnlineDBContext : DbContext
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
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Subject> Subjects { get; set; }
    } 
}