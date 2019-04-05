namespace GeneralEngineeringTechnologies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditProjecManagerValidation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ProjectManager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ProjectManager_Id" });
            AlterColumn("dbo.Projects", "ProjectManager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Projects", "ProjectManager_Id");
            AddForeignKey("dbo.Projects", "ProjectManager_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectManager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ProjectManager_Id" });
            AlterColumn("dbo.Projects", "ProjectManager_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "ProjectManager_Id");
            AddForeignKey("dbo.Projects", "ProjectManager_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
