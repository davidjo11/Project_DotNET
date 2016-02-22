namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesInc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomRoles", "RolesIncId", "dbo.RolesIncs");
            DropIndex("dbo.CustomRoles", new[] { "RolesIncId" });
            AddColumn("dbo.CustomRoles", "RolesInc_RolesIncId", c => c.Int());
            AddColumn("dbo.CustomRoles", "RolesInc_RolesIncId1", c => c.Int());
            AddColumn("dbo.RolesIncs", "BelongsTo_CustomRoleId", c => c.Int());
            CreateIndex("dbo.CustomRoles", "RolesInc_RolesIncId");
            CreateIndex("dbo.CustomRoles", "RolesInc_RolesIncId1");
            CreateIndex("dbo.RolesIncs", "BelongsTo_CustomRoleId");
            AddForeignKey("dbo.RolesIncs", "BelongsTo_CustomRoleId", "dbo.CustomRoles", "CustomRoleId");
            AddForeignKey("dbo.CustomRoles", "RolesInc_RolesIncId1", "dbo.RolesIncs", "RolesIncId");
            AddForeignKey("dbo.CustomRoles", "RolesInc_RolesIncId", "dbo.RolesIncs", "RolesIncId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomRoles", "RolesInc_RolesIncId", "dbo.RolesIncs");
            DropForeignKey("dbo.CustomRoles", "RolesInc_RolesIncId1", "dbo.RolesIncs");
            DropForeignKey("dbo.RolesIncs", "BelongsTo_CustomRoleId", "dbo.CustomRoles");
            DropIndex("dbo.RolesIncs", new[] { "BelongsTo_CustomRoleId" });
            DropIndex("dbo.CustomRoles", new[] { "RolesInc_RolesIncId1" });
            DropIndex("dbo.CustomRoles", new[] { "RolesInc_RolesIncId" });
            DropColumn("dbo.RolesIncs", "BelongsTo_CustomRoleId");
            DropColumn("dbo.CustomRoles", "RolesInc_RolesIncId1");
            DropColumn("dbo.CustomRoles", "RolesInc_RolesIncId");
            CreateIndex("dbo.CustomRoles", "RolesIncId");
            AddForeignKey("dbo.CustomRoles", "RolesIncId", "dbo.RolesIncs", "RolesIncId", cascadeDelete: true);
        }
    }
}
