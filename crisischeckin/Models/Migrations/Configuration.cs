namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Common;

    public sealed class Configuration : DbMigrationsConfiguration<Models.CrisisCheckin>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        public static void SeedIfNotEmpty(CrisisCheckin context) // Not overriding DbMigrationsConfiguration<T>.Seed, since it doesn't seem to always get called when it should.
        {
            // Since the previous migrations, we've added the 
            // Volunteer type table:
            if (!context.VolunteerTypes.Any())
            {
                context.VolunteerTypes.AddOrUpdate(
                    v => v.Name,
                    new VolunteerType { Name = "On Site" },
                    new VolunteerType { Name = "Remote" }
                );
            }

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

            context.Persons.AddOrUpdate(
                p => p.FirstName,
                new Person
                {
                    FirstName = "Bob",
                    Commitments =
                        new Commitment[] { new Commitment { StartDate = new DateTime(2014, 1, 1), EndDate = new DateTime(2014, 2, 1), Disaster = new Disaster { Name = "Hurricane", IsActive = true } } }
                });

            // Set up automated test user
            var testUser = context.Users.FirstOrDefault(u => u.UserName == Constants.DefaultTestUserName);
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
                                    Disaster = new Disaster { Name = "Test Disaster", IsActive = true } 
                                } 
                            }
                        });
                }              
            }

            context.SaveChanges();
        }
    }
}
