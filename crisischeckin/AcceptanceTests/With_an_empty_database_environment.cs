using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using Models;
using Models.Migrations;
using NUnit.Framework;

namespace AcceptanceTests
{
    public class With_an_empty_database_environment
    {

        [TestFixtureSetUp]
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
            dbContext.ClusterGroups.AddOrUpdate(
                g => g.Name, new ClusterGroup { Name = "UN Clusters", Description = "Some description here" });
            
            dbContext.Clusters.AddOrUpdate(
                c => c.Name,
                new Cluster { Name = "Agriculture Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Camp Coordination and Management Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Early Recovery Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Emergency Shelter Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Emergency Telecommunications Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Food Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Health Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Logistics Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Nutrition Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Protection Cluster", ClusterGroupId = 1 },
                new Cluster { Name = "Water and Sanitation Cluster", ClusterGroupId = 1 }
                );
            dbContext.SaveChanges();
        }
    }
}