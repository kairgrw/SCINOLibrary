namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr104 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bids", "Book_ID", c => c.Int());
            CreateIndex("dbo.Bids", "Book_ID");
            AddForeignKey("dbo.Bids", "Book_ID", "dbo.Books", "ID");
            DropColumn("dbo.Books", "IsBidCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "IsBidCreated", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Bids", "Book_ID", "dbo.Books");
            DropIndex("dbo.Bids", new[] { "Book_ID" });
            DropColumn("dbo.Bids", "Book_ID");
        }
    }
}
