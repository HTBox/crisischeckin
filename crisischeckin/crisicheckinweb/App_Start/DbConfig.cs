using System.Data.Entity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crisicheckinweb
{
    public class DbConfig
    {
        public static void Initialize()
        {
          //  Database.SetInitializer<CrisisCheckin>(
          //      new MigrateDatabaseToLatestVersion<CrisisCheckin, Models.Migrations.Configuration>());
        }
    }
}