using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<CrisisCheckin>(new DropCreateDatabaseAlways<CrisisCheckin>());

            using (var context = new CrisisCheckin())
            {
                if (!context.Database.Exists())
                {
                    ((IObjectContextAdapter) context).ObjectContext.CreateDatabase();
                }
            }
        }
    }
}