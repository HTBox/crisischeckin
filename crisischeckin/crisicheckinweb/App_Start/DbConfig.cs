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

            using (var db = new CrisisCheckin()) {
                if (!db.Database.CompatibleWithModel(false)) {
                    Database.SetInitializer<CrisisCheckin>(new MigrateDatabaseToLatestVersion<CrisisCheckin, Models.Migrations.Configuration>());
                    db.Database.Initialize(false);
                } 
                Models.Migrations.Configuration.SeedIfNotEmpty(db);
            }
        }
    }
}