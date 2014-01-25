using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace AcceptanceTests
{
    [TestClass]
    public class With_an_empty_database_environment
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            Database.SetInitializer<CrisisCheckin>(new MyInitializer());

            using (var context = new CrisisCheckin())
            {
                if (!context.Database.Exists())
                {
                    ((IObjectContextAdapter) context).ObjectContext.CreateDatabase();
                }
                context.Database.Initialize(true);
            }
        }
    }

    public class MyInitializer : IDatabaseInitializer<CrisisCheckin>
    {
        public void InitializeDatabase(CrisisCheckin context)
        {
            new DropCreateDatabaseAlways<CrisisCheckin>().InitializeDatabase(context);
            Seed(context);
        }

        protected void Seed(CrisisCheckin context)
        {
            context.Clusters.Add(new Cluster {Name = "Agriculture Cluster"});
            context.Clusters.Add(new Cluster {Name = "Camp Coordination and Management Cluster"});
            context.Clusters.Add(new Cluster {Name = "Early Recovery Cluster"});
            context.Clusters.Add(new Cluster {Name = "Emergency Shelter Cluster"});
            context.Clusters.Add(new Cluster {Name = "Emergency Telecommunications Cluster"});
            context.Clusters.Add(new Cluster {Name = "Food Cluster"});
            context.Clusters.Add(new Cluster {Name = "Health Cluster"});
            context.Clusters.Add(new Cluster {Name = "Logistics Cluster"});
            context.Clusters.Add(new Cluster {Name = "Nutrition Cluster"});
            context.Clusters.Add(new Cluster {Name = "Protection Cluster"});
            context.Clusters.Add(new Cluster {Name = "Water and Sanitation Cluster"});

            context.SaveChanges();
        }
    }
}