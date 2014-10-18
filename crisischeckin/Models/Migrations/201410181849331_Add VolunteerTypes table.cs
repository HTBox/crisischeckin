namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddVolunteerTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerTypes", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.VolunteerTypes", "Name", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.VolunteerTypes", "Id");
            DropColumn("dbo.VolunteerTypes", "Name");
        }
    }
}
