namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Common;

    public sealed class CrisisCheckinConfiguration : DbMigrationsConfiguration<Models.CrisisCheckin> {
        public CrisisCheckinConfiguration() {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(CrisisCheckin context)
        {
            // Populate resource types
            context.ResourceTypes.AddOrUpdate(x => x.ResourceTypeId,
                new ResourceType { ResourceTypeId = 1, TypeName = "Resource-Food" },
                new ResourceType { ResourceTypeId = 2, TypeName = "Water"},
                new ResourceType { ResourceTypeId = 3, TypeName = "Medical"},
                new ResourceType { ResourceTypeId = 4, TypeName = "Shelter"},
                new ResourceType { ResourceTypeId = 5, TypeName = "Heavy Machines"},
                new ResourceType { ResourceTypeId = 6, TypeName = "Hazard" },
                new ResourceType { ResourceTypeId = 7, TypeName = "Pamphlets Delivered" },
                new ResourceType { ResourceTypeId = 8, TypeName = "Medical Services" });

        }
    }
}
