namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBids",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Bids", "UserBid_ID", c => c.Int());
            CreateIndex("dbo.Bids", "UserBid_ID");
            AddForeignKey("dbo.Bids", "UserBid_ID", "dbo.UserBids", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bids", "UserBid_ID", "dbo.UserBids");
            DropIndex("dbo.Bids", new[] { "UserBid_ID" });
            DropColumn("dbo.Bids", "UserBid_ID");
            DropTable("dbo.UserBids");
        }
    }
}
