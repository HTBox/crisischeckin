namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(maxLength: 50),
                        Location_BuildingName = c.String(maxLength: 30),
                        Location_AddressLine1 = c.String(maxLength: 40),
                        Location_AddressLine2 = c.String(maxLength: 40),
                        Location_AddressLine3 = c.String(maxLength: 40),
                        Location_City = c.String(maxLength: 30),
                        Location_County = c.String(maxLength: 30),
                        Location_State = c.String(maxLength: 30),
                        Location_Country = c.String(maxLength: 30),
                        Location_PostalCode = c.String(maxLength: 15),
                        Type = c.Int(nullable: false),
                        PocPerson_Id = c.Int(),
                    })
                .PrimaryKey(t => t.OrganizationId)
                .ForeignKey("dbo.Person", t => t.PocPerson_Id)
                .Index(t => t.PocPerson_Id);
            
            CreateTable(
                "dbo.OrganizationDisaster",
                c => new
                    {
                        Organization_OrganizationId = c.Int(nullable: false),
                        Disaster_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrganizationId, t.Disaster_Id })
                .ForeignKey("dbo.Organization", t => t.Organization_OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Disaster", t => t.Disaster_Id, cascadeDelete: true)
                .Index(t => t.Organization_OrganizationId)
                .Index(t => t.Disaster_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "PocPerson_Id", "dbo.Person");
            DropForeignKey("dbo.OrganizationDisaster", "Disaster_Id", "dbo.Disaster");
            DropForeignKey("dbo.OrganizationDisaster", "Organization_OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationDisaster", new[] { "Disaster_Id" });
            DropIndex("dbo.OrganizationDisaster", new[] { "Organization_OrganizationId" });
            DropIndex("dbo.Organization", new[] { "PocPerson_Id" });
            DropTable("dbo.OrganizationDisaster");
            DropTable("dbo.Organization");
        }
    }
}
