namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonToResource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resource", "PersonId", c => c.Int());
            CreateIndex("dbo.Resource", "PersonId");
            AddForeignKey("dbo.Resource", "PersonId", "dbo.Person", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resource", "PersonId", "dbo.Person");
            DropIndex("dbo.Resource", new[] { "PersonId" });
            DropColumn("dbo.Resource", "PersonId");
        }
    }
}
