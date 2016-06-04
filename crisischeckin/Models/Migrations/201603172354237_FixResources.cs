namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixResources : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrganizationResource", "Organization_OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationResource", new[] { "Organization_OrganizationId" });
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        StartOfAvailability = c.DateTime(nullable: false),
                        EndOfAvailability = c.DateTime(nullable: false),
                        Location_BuildingName = c.String(maxLength: 30),
                        Location_AddressLine1 = c.String(maxLength: 40),
                        Location_AddressLine2 = c.String(maxLength: 40),
                        Location_AddressLine3 = c.String(maxLength: 40),
                        Location_City = c.String(maxLength: 30),
                        Location_County = c.String(maxLength: 30),
                        Location_State = c.String(maxLength: 30),
                        Location_Country = c.String(maxLength: 30),
                        Location_PostalCode = c.String(maxLength: 15),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        Allocator_OrganizationId = c.Int(),
                        Disaster_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Organization", t => t.Allocator_OrganizationId)
                .ForeignKey("dbo.Disaster", t => t.Disaster_Id)
                .Index(t => t.Allocator_OrganizationId)
                .Index(t => t.Disaster_Id);
            
            CreateTable(
                "dbo.ResourceType",
                c => new
                    {
                        ResourceTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.ResourceTypeId);
            
            DropTable("dbo.OrganizationResource");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrganizationResource",
                c => new
                    {
                        OrganizationResourceId = c.Int(nullable: false, identity: true),
                        ResourceTitle = c.String(maxLength: 15),
                        Data = c.Binary(),
                        Organization_OrganizationId = c.Int(),
                    })
                .PrimaryKey(t => t.OrganizationResourceId);
            
            DropForeignKey("dbo.Resource", "Disaster_Id", "dbo.Disaster");
            DropForeignKey("dbo.Resource", "Allocator_OrganizationId", "dbo.Organization");
            DropIndex("dbo.Resource", new[] { "Disaster_Id" });
            DropIndex("dbo.Resource", new[] { "Allocator_OrganizationId" });
            DropTable("dbo.ResourceType");
            DropTable("dbo.Resource");
            CreateIndex("dbo.OrganizationResource", "Organization_OrganizationId");
            AddForeignKey("dbo.OrganizationResource", "Organization_OrganizationId", "dbo.Organization", "OrganizationId");
        }
    }
}
