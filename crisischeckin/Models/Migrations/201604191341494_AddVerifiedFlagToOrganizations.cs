namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerifiedFlagToOrganizations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "Verified", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "Verified");
        }
    }
}
