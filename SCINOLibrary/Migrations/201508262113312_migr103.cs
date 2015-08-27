namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr103 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bids", "UserBid_ID", "dbo.UserBids");
            DropForeignKey("dbo.UserBids", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Bids", new[] { "UserBid_ID" });
            DropIndex("dbo.UserBids", new[] { "User_Id" });
            DropColumn("dbo.Bids", "UserBid_ID");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserBids",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Bids", "UserBid_ID", c => c.Int());
            CreateIndex("dbo.UserBids", "User_Id");
            CreateIndex("dbo.Bids", "UserBid_ID");
            AddForeignKey("dbo.UserBids", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Bids", "UserBid_ID", "dbo.UserBids", "ID");
        }
    }
}
