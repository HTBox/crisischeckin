namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireClusterName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cluster", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cluster", "Name", c => c.String());
        }
    }
}
