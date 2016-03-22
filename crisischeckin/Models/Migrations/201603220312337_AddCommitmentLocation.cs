namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommitmentLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commitment", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commitment", "Location");
        }
    }
}
