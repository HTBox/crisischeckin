using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.Migrations;

namespace AcceptanceTests
{
    [TestClass]
    public class With_an_empty_database_environment
    {

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            using (var dbContext = new CrisisCheckin())
            {
                dbContext.Database.Delete();

                new MigrateDatabaseToLatestVersion<CrisisCheckin, CrisisCheckinConfiguration>()
                    .InitializeDatabase(dbContext);

                Seed(dbContext);
            }
        }

        static void Seed(CrisisCheckin dbContext)
        {
            dbContext.Clusters.AddOrUpdate(
                c => c.Name,
                new Cluster { Name = "Agriculture Cluster" },
                new Cluster { Name = "Camp Coordination and Management Cluster" },
                new Cluster { Name = "Early Recovery Cluster" },
                new Cluster { Name = "Emergency Shelter Cluster" },
                new Cluster { Name = "Emergency Telecommunications Cluster" },
                new Cluster { Name = "Food Cluster" },
                new Cluster { Name = "Health Cluster" },
                new Cluster { Name = "Logistics Cluster" },
                new Cluster { Name = "Nutrition Cluster" },
                new Cluster { Name = "Protection Cluster" },
                new Cluster { Name = "Water and Sanitation Cluster" }
                );
            dbContext.SaveChanges();
        }
    }
}