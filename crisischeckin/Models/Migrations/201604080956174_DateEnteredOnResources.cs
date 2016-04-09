namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateEnteredOnResources : DbMigration
    {
        public override void Up()
        {
            var currentTime = DateTime.Now;
            AddColumn("dbo.Resource", "EntryMade", c => c.DateTime(nullable: false, defaultValue: currentTime));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resource", "EntryMade");
        }
    }
}
