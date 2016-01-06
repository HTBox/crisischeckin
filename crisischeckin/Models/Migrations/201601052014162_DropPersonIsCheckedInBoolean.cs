namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropPersonIsCheckedInBoolean : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Commitment", "PersonIsCheckedIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Commitment", "PersonIsCheckedIn", c => c.Boolean(nullable: false));
        }
    }
}
