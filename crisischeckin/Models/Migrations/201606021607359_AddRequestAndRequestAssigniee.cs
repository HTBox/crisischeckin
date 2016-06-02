namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestAndRequestAssigniee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestAssigniee",
                c => new
                    {
                        RequestAssignieeId = c.Int(nullable: false, identity: true),
                        AssignieeId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false),
                        Request_RequestId = c.Int(),
                        Request_RequestId1 = c.Int(),
                        Request_RequestId2 = c.Int(),
                    })
                .PrimaryKey(t => t.RequestAssignieeId)
                .ForeignKey("dbo.Person", t => t.AssignieeId, cascadeDelete: true)
                .ForeignKey("dbo.Request", t => t.Request_RequestId)
                .ForeignKey("dbo.Request", t => t.Request_RequestId1)
                .ForeignKey("dbo.Request", t => t.Request_RequestId2)
                .Index(t => t.AssignieeId)
                .Index(t => t.Request_RequestId)
                .Index(t => t.Request_RequestId1)
                .Index(t => t.Request_RequestId2);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        OrganizationId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Person", t => t.CreatorId, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestAssigniee", "Request_RequestId2", "dbo.Request");
            DropForeignKey("dbo.Request", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Request", "CreatorId", "dbo.Person");
            DropForeignKey("dbo.RequestAssigniee", "Request_RequestId1", "dbo.Request");
            DropForeignKey("dbo.RequestAssigniee", "Request_RequestId", "dbo.Request");
            DropForeignKey("dbo.RequestAssigniee", "AssignieeId", "dbo.Person");
            DropIndex("dbo.Request", new[] { "CreatorId" });
            DropIndex("dbo.Request", new[] { "OrganizationId" });
            DropIndex("dbo.RequestAssigniee", new[] { "Request_RequestId2" });
            DropIndex("dbo.RequestAssigniee", new[] { "Request_RequestId1" });
            DropIndex("dbo.RequestAssigniee", new[] { "Request_RequestId" });
            DropIndex("dbo.RequestAssigniee", new[] { "AssignieeId" });
            DropTable("dbo.Request");
            DropTable("dbo.RequestAssigniee");
        }
    }
}
