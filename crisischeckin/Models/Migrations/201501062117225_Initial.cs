namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClusterCoordinatorLogEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStampUtc = c.DateTime(nullable: false),
                        Event = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        PersonName = c.String(),
                        DisasterId = c.Int(nullable: false),
                        DisasterName = c.String(),
                        ClusterId = c.Int(nullable: false),
                        ClusterName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClusterCoordinator",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        DisasterId = c.Int(nullable: false),
                        ClusterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cluster", t => t.ClusterId, cascadeDelete: true)
                .ForeignKey("dbo.Disaster", t => t.DisasterId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.DisasterId)
                .Index(t => t.ClusterId);
            
            CreateTable(
                "dbo.Cluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ClusterId = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cluster", t => t.ClusterId)
                .Index(t => t.ClusterId);
            
            CreateTable(
                "dbo.Commitment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DisasterId = c.Int(nullable: false),
                        PersonIsCheckedIn = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disaster", t => t.DisasterId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.DisasterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClusterCoordinator", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Commitment", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Commitment", "DisasterId", "dbo.Disaster");
            DropForeignKey("dbo.Person", "ClusterId", "dbo.Cluster");
            DropForeignKey("dbo.ClusterCoordinator", "DisasterId", "dbo.Disaster");
            DropForeignKey("dbo.ClusterCoordinator", "ClusterId", "dbo.Cluster");
            DropIndex("dbo.Commitment", new[] { "DisasterId" });
            DropIndex("dbo.Commitment", new[] { "PersonId" });
            DropIndex("dbo.Person", new[] { "ClusterId" });
            DropIndex("dbo.ClusterCoordinator", new[] { "ClusterId" });
            DropIndex("dbo.ClusterCoordinator", new[] { "DisasterId" });
            DropIndex("dbo.ClusterCoordinator", new[] { "PersonId" });
            DropTable("dbo.Commitment");
            DropTable("dbo.Person");
            DropTable("dbo.Disaster");
            DropTable("dbo.Cluster");
            DropTable("dbo.ClusterCoordinator");
            DropTable("dbo.ClusterCoordinatorLogEntry");
        }
    }
}
