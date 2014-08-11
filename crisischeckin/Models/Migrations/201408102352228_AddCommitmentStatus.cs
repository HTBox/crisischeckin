namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommitmentStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commitment", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commitment", "Status");
        }
    }
}
