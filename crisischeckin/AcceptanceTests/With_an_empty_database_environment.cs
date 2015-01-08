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
            //TODO: This test is invalid and needs to be rewritten or thrown away with new migrations scheme
            //AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            //Database.SetInitializer<CrisisCheckin>(new Initializer());

            //using (var context = new CrisisCheckin())
            //{
            //    context.Database.Delete();
            //    context.Database.Initialize(true);
            //}
        }

        class Initializer : MigrateDatabaseToLatestVersion<CrisisCheckin, Models.Migrations.CrisisCheckinConfiguration>
        {
            public override void InitializeDatabase(CrisisCheckin context)
            {
                //TODO: This test is invalid and needs to be rewritten or thrown away with new migrations scheme
                //base.InitializeDatabase(context);
                //Models.Migrations.Configuration.SeedIfNotEmpty(context);
            }
        }
    }
}