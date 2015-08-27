namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr95 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        IsReviewed = c.Boolean(nullable: false),
                        CreateAt = c.DateTime(nullable: false),
                        ReviewAt = c.DateTime(),
                        BookToBuy_ID = c.Int(),
                        SuggestedBook_ID = c.Int(),
                        WantedBook_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.BookToBuy_ID)
                .ForeignKey("dbo.Books", t => t.SuggestedBook_ID)
                .ForeignKey("dbo.Books", t => t.WantedBook_ID)
                .Index(t => t.BookToBuy_ID)
                .Index(t => t.SuggestedBook_ID)
                .Index(t => t.WantedBook_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bids", "WantedBook_ID", "dbo.Books");
            DropForeignKey("dbo.Bids", "SuggestedBook_ID", "dbo.Books");
            DropForeignKey("dbo.Bids", "BookToBuy_ID", "dbo.Books");
            DropIndex("dbo.Bids", new[] { "WantedBook_ID" });
            DropIndex("dbo.Bids", new[] { "SuggestedBook_ID" });
            DropIndex("dbo.Bids", new[] { "BookToBuy_ID" });
            DropTable("dbo.Bids");
        }
    }
}
