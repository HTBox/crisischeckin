using Common;
using Models;
using Models.Migrations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

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
            // We want to call this method even when the database updates aren't necessary. That's
            // because VS 2013 tooling automatically creates the DB when the app starts. Therefore,
            // this code only executes when the clusters table is empty. That's a good proxy for a
            // clean database with no data.
            if (context.Clusters.Any())
                return;

            context.ClusterGroups.AddOrUpdate(g => g.Name, 
                new ClusterGroup { Name = "UN Clusters", Description = "The UN Cluster list." });

            context.SaveChanges();
            var clusterGroup = context.ClusterGroups.First();

            context.Clusters.AddOrUpdate(
                c => c.Name,
                new Cluster { Name = "Agriculture Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Camp Coordination and Management Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Early Recovery Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Emergency Shelter Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Emergency Telecommunications Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Food Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Health Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Logistics Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Nutrition Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Protection Cluster", ClusterGroupId = clusterGroup.Id },
                new Cluster { Name = "Water and Sanitation Cluster", ClusterGroupId = clusterGroup.Id }
                );
            context.SaveChanges();
            var vtype = context.VolunteerTypes.First(vt => vt.Name == VolunteerType.VOLUNTEERTYPE_ONSITE);

            var firstCluster = context.Clusters.FirstOrDefault();

            context.Persons.AddOrUpdate(
                p => p.FirstName,
                new Person
                {
                    FirstName = "Bob",
                    Commitments =
                        new Commitment[] { new Commitment { StartDate = new DateTime(2014, 1, 1), EndDate = new DateTime(2014, 2, 1), Disaster = new Disaster { Name = "Hurricane", IsActive = true }, VolunteerType = vtype, Cluster = firstCluster } }
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
                        Commitments = new Commitment[]
                            {
                                new Commitment
                                {
                                    StartDate = new DateTime(DateTime.Now.Year, 1, 1), EndDate = new DateTime(DateTime.Now.Year, 2, 1),
                                    Disaster = new Disaster { Name = "Test Disaster", IsActive = true },
                                    VolunteerType = vtype, Cluster = firstCluster
                                }
                            },
						OrganizationId = 1,
 
                        Organization = new Organization
                            {
								OrganizationName = "Humanitarian Toolbox",
								OrganizationId = 1,
								Type = OrganizationTypeEnum.NonProfit,
								Location = new Address
								{
									AddressLine1 = "HT AddressLine 1",
									AddressLine2 = "HT Address Line 2",
									AddressLine3 = "HT Address Line 3",
									BuildingName = "HT Building Name",
									City = "HTCity",
									Country = "USA",
									County = "HT County",
									PostalCode = "12345",
									State = "HT State"
								},
								Verified = true
							}

                    });
                }
            }

            context.SaveChanges();

            context.DisasterClusters.Add(new DisasterCluster { DisasterId = 1, ClusterId = 1 });
            context.DisasterClusters.Add(new DisasterCluster { DisasterId = 2, ClusterId = 2 });

            context.SaveChanges();
        }
    }
}