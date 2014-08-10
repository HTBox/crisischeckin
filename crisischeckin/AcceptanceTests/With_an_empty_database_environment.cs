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
            Database.SetInitializer<CrisisCheckin>(new Initializer());

            using (var context = new CrisisCheckin())
            {
                context.Database.Delete();
                context.Database.Initialize(true);
            }
        }

        class Initializer : MigrateDatabaseToLatestVersion<CrisisCheckin, Models.Migrations.Configuration>
        {
            public override void InitializeDatabase(CrisisCheckin context)
            {
                base.InitializeDatabase(context);
                Models.Migrations.Configuration.SeedIfNotEmpty(context);
            }
        }
    }
}