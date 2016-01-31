namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDesc1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Periods", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Periods", new[] { "User_Id" });
            AddColumn("dbo.Periods", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Periods", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Periods", "User_Id");
            AddForeignKey("dbo.Periods", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Periods", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Periods", new[] { "User_Id" });
            AlterColumn("dbo.Periods", "User_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Periods", "UserId");
            CreateIndex("dbo.Periods", "User_Id");
            AddForeignKey("dbo.Periods", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
