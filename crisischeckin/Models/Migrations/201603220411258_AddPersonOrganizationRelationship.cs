namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonOrganizationRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organization", "PocPerson_Id", "dbo.Person");
            DropIndex("dbo.Organization", new[] { "PocPerson_Id" });
            AddColumn("dbo.Person", "OrganizationId", c => c.Int());
            CreateIndex("dbo.Person", "OrganizationId");
            AddForeignKey("dbo.Person", "OrganizationId", "dbo.Organization", "OrganizationId");
            DropColumn("dbo.Organization", "PocPerson_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organization", "PocPerson_Id", c => c.Int());
            DropForeignKey("dbo.Person", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.Person", new[] { "OrganizationId" });
            DropColumn("dbo.Person", "OrganizationId");
            CreateIndex("dbo.Organization", "PocPerson_Id");
            AddForeignKey("dbo.Organization", "PocPerson_Id", "dbo.Person", "Id");
        }
    }
}
