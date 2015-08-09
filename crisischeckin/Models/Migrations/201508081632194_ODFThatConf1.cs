namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ODFThatConf1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Person", name: "ClusterId", newName: "Cluster_Id");
            RenameIndex(table: "dbo.Person", name: "IX_ClusterId", newName: "IX_Cluster_Id");
            AddColumn("dbo.Commitment", "ClusterId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commitment", "ClusterId");
            RenameIndex(table: "dbo.Person", name: "IX_Cluster_Id", newName: "IX_ClusterId");
            RenameColumn(table: "dbo.Person", name: "Cluster_Id", newName: "ClusterId");
        }
    }
}
