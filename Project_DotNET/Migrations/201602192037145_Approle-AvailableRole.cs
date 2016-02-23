namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproleAvailableRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        AppRoleId = c.Int(nullable: false, identity: true),
                        AppRoleName = c.String(),
                        AppRoleDesc = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AppRoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AvailableRoles",
                c => new
                    {
                        AvailableRoleId = c.Int(nullable: false, identity: true),
                        AvailableRoleName = c.String(),
                        AvailableRoleDesc = c.String(),
                        AppRole_AppRoleId = c.Int(),
                    })
                .PrimaryKey(t => t.AvailableRoleId)
                .ForeignKey("dbo.AppRoles", t => t.AppRole_AppRoleId)
                .Index(t => t.AppRole_AppRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvailableRoles", "AppRole_AppRoleId", "dbo.AppRoles");
            DropIndex("dbo.AvailableRoles", new[] { "AppRole_AppRoleId" });
            DropIndex("dbo.AppRoles", new[] { "ApplicationUser_Id" });
            DropTable("dbo.AvailableRoles");
            DropTable("dbo.AppRoles");
        }
    }
}
