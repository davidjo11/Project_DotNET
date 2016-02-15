namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
            AlterColumn("dbo.Jobs", "JobName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "firstName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "lastName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "lastName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "firstName", c => c.String(nullable: false));
            AlterColumn("dbo.Jobs", "JobName", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false));
        }
    }
}
