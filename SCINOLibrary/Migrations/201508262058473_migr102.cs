namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr102 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserBids", new[] { "user_Id" });
            CreateIndex("dbo.UserBids", "User_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserBids", new[] { "User_Id" });
            CreateIndex("dbo.UserBids", "user_Id");
        }
    }
}
