namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBids", "user_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserBids", "user_Id");
            AddForeignKey("dbo.UserBids", "user_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.UserBids", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserBids", "UserId", c => c.String());
            DropForeignKey("dbo.UserBids", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserBids", new[] { "user_Id" });
            DropColumn("dbo.UserBids", "user_Id");
        }
    }
}
