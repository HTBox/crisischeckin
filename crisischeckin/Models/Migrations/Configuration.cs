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
    }
}
