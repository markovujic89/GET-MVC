namespace GeneralEngineeringTechnologies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editProperty : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tasks", name: "Assigne_Id", newName: "AssignedUser_Id");
            RenameIndex(table: "dbo.Tasks", name: "IX_Assigne_Id", newName: "IX_AssignedUser_Id");
            AddColumn("dbo.Tasks", "Progress", c => c.Int(nullable: false));
            DropColumn("dbo.Tasks", "Proggres");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Proggres", c => c.Int(nullable: false));
            DropColumn("dbo.Tasks", "Progress");
            RenameIndex(table: "dbo.Tasks", name: "IX_AssignedUser_Id", newName: "IX_Assigne_Id");
            RenameColumn(table: "dbo.Tasks", name: "AssignedUser_Id", newName: "Assigne_Id");
        }
    }
}
