using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

using Common;
using Models;
using System.Configuration;
using Models.Migrations;

namespace crisicheckinweb
{
    public class DbConfig
    {
        public static void InitializeCrisisCheckinAndMembershipData()
        {
            var migrateOnStartup = false;
            if (!bool.TryParse(ConfigurationManager.AppSettings["MigrateDbToLatestOnStartup"], out migrateOnStartup))
            {
                migrateOnStartup = false;
            }

            if (migrateOnStartup)
            {
                Database.SetInitializer<CrisisCheckin>(new MigrateDatabaseToLatestVersion<CrisisCheckin, Models.Migrations.CrisisCheckinConfiguration>());
            }
            else
            {
                Database.SetInitializer<CrisisCheckin>(null);
            }

            //Users are created via membership so never ever do initialization
            Database.SetInitializer<CrisisCheckinMembership>(null);

            using (var db = new CrisisCheckin())
            {
                db.Database.CreateIfNotExists();

                if (migrateOnStartup)
                {
                    var configuration = new CrisisCheckinConfiguration();

                    var migrator = new DbMigrator(configuration);
                    migrator.Update();
                }

                AuthConfig.Register();
                AuthConfig.VerifyRolesAndDefaultAdminAccount();

                using (var mdb = new CrisisCheckinMembership())
                {
                    SeedIfNotEmpty(db, mdb);
                }
            }
        }

        public static void SeedIfNotEmpty(CrisisCheckin context, CrisisCheckinMembership membership_context) // Not overriding DbMigrationsConfiguration<T>.Seed, since it doesn't seem to always get called when it should.
        {
            // We want to call this method even when the database
            // updates aren't necessary. That's because VS 2013 
            // tooling automatically creates the DB when the app
            // starts. Therefore, this code only executes 
            // when the clusters table is empty.  That's a good
            // proxy for a clean database with no data.
            if (context.Clusters.Any())
                return;

            context.Clusters.AddOrUpdate(
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
            context.SaveChanges();
            var vtype = context.VolunteerTypes.First(vt => vt.Name == VolunteerType.VOLUNTEERTYPE_ONSITE);

            context.Persons.AddOrUpdate(
                p => p.FirstName,
                new Person
                {
                    FirstName = "Bob",
                    Commitments =
                        new Commitment[] { new Commitment { StartDate = new DateTime(2014, 1, 1), EndDate = new DateTime(2014, 2, 1), Disaster = new Disaster { Name = "Hurricane", IsActive = true }, VolunteerType = vtype } }
                });

            // Set up automated test user
            var testUser = membership_context.Users.FirstOrDefault(u => u.UserName == Constants.DefaultTestUserName);
            if (testUser != null)
            {
                if (context.Persons.FirstOrDefault(p => p.UserId == testUser.Id) == null)
                {
                    context.Persons.Add(new Person
                    {
                        UserId = testUser.Id,
                        FirstName = "Test",
                        LastName = "User",
                        Email = "TestUser@htbox.org",
                        Cluster = context.Clusters.FirstOrDefault(cluster => cluster.Name == "Agriculture Cluster"),
                        Commitments = new Commitment[] 
                            { 
                                new Commitment 
                                { 
                                    StartDate = new DateTime(DateTime.Now.Year, 1, 1), EndDate = new DateTime(DateTime.Now.Year, 2, 1), 
                                    Disaster = new Disaster { Name = "Test Disaster", IsActive = true },
                                    VolunteerType = vtype
                                } 
                            }
                    });
                }
            }

            context.SaveChanges();
        }

    }
}