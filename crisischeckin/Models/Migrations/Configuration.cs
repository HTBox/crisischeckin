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
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.CrisisCheckin context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Disasters.AddOrUpdate(d => d.Id,
                new Disaster { Id = 1, Name = "Fake Disaster 1" },
                new Disaster { Id = 2, Name = "Fake Disaster 2" }
            );

            context.Users.AddOrUpdate(u => u.Id,
                new User { Id = 1, UserName = "ctester" }
            );

            context.Persons.AddOrUpdate(p => p.Id,
                new Person { Id = 1, UserId = 1, FirstName = "Chester", LastName = "Tester", Email = "ctester@test.com", PhoneNumber = "608-555-1212"},
                new Person { Id = 2, FirstName = "Lester", LastName = "Tester", Email = "ltester@test.com", PhoneNumber = "608-555-1212" }
            );

            context.Commitments.AddOrUpdate(c => c.Id, 
                new Commitment { Id = 1, DisasterId = 1, PersonId = 1, StartDate = new DateTime(2013, 8, 10), EndDate = new DateTime(2013, 8, 14) },
                new Commitment { Id = 2, DisasterId = 1, PersonId = 2, StartDate = new DateTime(2013, 8, 12), EndDate = new DateTime(2013, 8, 15) },
                new Commitment { Id = 3, DisasterId = 2, PersonId = 1, StartDate = new DateTime(2013, 9, 1), EndDate = new DateTime(2013, 9, 5) }
            );

        }
    }
}
