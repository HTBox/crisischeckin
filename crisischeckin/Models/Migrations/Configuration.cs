namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Models.CrisisCheckin>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public static void SeedIfNotEmpty(CrisisCheckin context) // Not overriding DbMigrationsConfiguration<T>.Seed, since it doesn't seem to always get called when it should.
        {
            context.Clusters.Add(new Cluster { Name = "Agriculture Cluster" });
            context.Clusters.Add(new Cluster { Name = "Camp Coordination and Management Cluster" });
            context.Clusters.Add(new Cluster { Name = "Early Recovery Cluster" });
            context.Clusters.Add(new Cluster { Name = "Emergency Shelter Cluster" });
            context.Clusters.Add(new Cluster { Name = "Emergency Telecommunications Cluster" });
            context.Clusters.Add(new Cluster { Name = "Food Cluster" });
            context.Clusters.Add(new Cluster { Name = "Health Cluster" });
            context.Clusters.Add(new Cluster { Name = "Logistics Cluster" });
            context.Clusters.Add(new Cluster { Name = "Nutrition Cluster" });
            context.Clusters.Add(new Cluster { Name = "Protection Cluster" });
            context.Clusters.Add(new Cluster { Name = "Water and Sanitation Cluster" });

            context.Persons.Add(new Person {
                FirstName = "Bob", Commitments =
                    new Commitment[] { new Commitment { StartDate = new DateTime(2014, 1, 1), EndDate = new DateTime(2014, 2, 1), Disaster = new Disaster { Name = "Hurricane", IsActive = true } } }
            });

            context.SaveChanges();
        }
    }
}
