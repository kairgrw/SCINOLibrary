namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr106 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bids", "Book_ID", c => c.Int());
            CreateIndex("dbo.Bids", "Book_ID");
            AddForeignKey("dbo.Bids", "Book_ID", "dbo.Books", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bids", "Book_ID", "dbo.Books");
            DropIndex("dbo.Bids", new[] { "Book_ID" });
            DropColumn("dbo.Bids", "Book_ID");
        }
    }
}
