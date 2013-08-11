namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClusters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Person", "ClusterId", c => c.Int());
            AddForeignKey("dbo.Commitment", "DisasterId", "dbo.Disaster", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Person", "ClusterId", "dbo.Cluster", "Id");
            CreateIndex("dbo.Commitment", "DisasterId");
            CreateIndex("dbo.Person", "ClusterId");

            Sql("INSERT INTO Cluster (Name) VALUES ('Agriculture Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Camp Coordination and Management Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Early Recovery Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Emergency Shelter Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Emergency Telecommunications Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Food Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Health Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Logistics Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Nutrition Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Protection Cluster');");
            Sql("INSERT INTO Cluster (Name) VALUES ('Water and Sanitation Cluster');");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Person", new[] { "ClusterId" });
            DropIndex("dbo.Commitment", new[] { "DisasterId" });
            DropForeignKey("dbo.Person", "ClusterId", "dbo.Cluster");
            DropForeignKey("dbo.Commitment", "DisasterId", "dbo.Disaster");
            DropColumn("dbo.Person", "ClusterId");
            DropTable("dbo.Cluster");
        }
    }
}
