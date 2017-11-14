using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PrivateTutorOnline.Models
{
    public class TutorDBInitializer : CreateDatabaseIfNotExists<TutorOnlineDBContext>
    {
        protected override void Seed(TutorOnlineDBContext context)
        {
            base.Seed(context);
        }
    }
}