namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        CreatorId = c.Int(nullable: false),
                        AssigneeId = c.Int(),
                        OrganizationId = c.Int(),
                        Completed = c.Boolean(nullable: false),
                        Location = c.String(),
                        RequestStatus = c.Int(),
                        NullableEndDate = c.DateTime(),
                        NullableCreatedDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Person_Id = c.Int(),
                        Person_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Person", t => t.AssigneeId)
                .ForeignKey("dbo.Person", t => t.CreatorId, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .ForeignKey("dbo.Person", t => t.Person_Id1)
                .Index(t => t.CreatorId)
                .Index(t => t.AssigneeId)
                .Index(t => t.OrganizationId)
                .Index(t => t.Person_Id)
                .Index(t => t.Person_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "Person_Id1", "dbo.Person");
            DropForeignKey("dbo.Request", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.Request", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Request", "CreatorId", "dbo.Person");
            DropForeignKey("dbo.Request", "AssigneeId", "dbo.Person");
            DropIndex("dbo.Request", new[] { "Person_Id1" });
            DropIndex("dbo.Request", new[] { "Person_Id" });
            DropIndex("dbo.Request", new[] { "OrganizationId" });
            DropIndex("dbo.Request", new[] { "AssigneeId" });
            DropIndex("dbo.Request", new[] { "CreatorId" });
            DropTable("dbo.Request");
        }
    }
}
