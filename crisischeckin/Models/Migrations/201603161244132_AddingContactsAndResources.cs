namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingContactsAndResources : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 15),
                        Organization_OrganizationId = c.Int(),
                        Person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.Organization", t => t.Organization_OrganizationId)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Organization_OrganizationId)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        OrganizationResourceId = c.Int(nullable: false, identity: true),
                        ResourceTitle = c.String(maxLength: 15),
                        Data = c.Binary(),
                        Organization_OrganizationId = c.Int(),
                    })
                .PrimaryKey(t => t.OrganizationResourceId)
                .ForeignKey("dbo.Organization", t => t.Organization_OrganizationId)
                .Index(t => t.Organization_OrganizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resource", "Organization_OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Contact", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.Contact", "Organization_OrganizationId", "dbo.Organization");
            DropIndex("dbo.Resource", new[] { "Organization_OrganizationId" });
            DropIndex("dbo.Contact", new[] { "Person_Id" });
            DropIndex("dbo.Contact", new[] { "Organization_OrganizationId" });
            DropTable("dbo.Resource");
            DropTable("dbo.Contact");
        }
    }
}
