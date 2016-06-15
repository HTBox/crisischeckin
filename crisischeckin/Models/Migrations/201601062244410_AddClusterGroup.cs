namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClusterGroup : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Cluster NOCHECK CONSTRAINT ALL");
            CreateTable(
                "dbo.ClusterGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            Sql("SET IDENTITY_INSERT dbo.ClusterGroup ON");
            Sql("INSERT INTO dbo.ClusterGroup (Id, Name, Description) VALUES (1, 'UN Clusters', 'Description here')");
            Sql("SET IDENTITY_INSERT dbo.ClusterGroup OFF");

            AddColumn("dbo.Cluster", "ClusterGroupId", c => c.Int(nullable: false));
            Sql("UPDATE dbo.Cluster SET ClusterGroupId = 1");
            CreateIndex("dbo.Cluster", "ClusterGroupId");

            AddForeignKey("dbo.Cluster", "ClusterGroupId", "dbo.ClusterGroup", "Id", cascadeDelete: true);

            Sql("ALTER TABLE dbo.Cluster CHECK CONSTRAINT ALL");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cluster", "ClusterGroupId", "dbo.ClusterGroup");
            DropIndex("dbo.Cluster", new[] { "ClusterGroupId" });
            DropColumn("dbo.Cluster", "ClusterGroupId");
            DropTable("dbo.ClusterGroup");
        }
    }
}
