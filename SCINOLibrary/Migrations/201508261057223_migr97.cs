namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr97 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bids", "IsChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bids", "CheckedAt", c => c.DateTime());
            AddColumn("dbo.Bids", "UserCreate_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bids", "UserCreate_Id");
            AddForeignKey("dbo.Bids", "UserCreate_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Bids", "IsReviewed");
            DropColumn("dbo.Bids", "ReviewAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bids", "ReviewAt", c => c.DateTime());
            AddColumn("dbo.Bids", "IsReviewed", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Bids", "UserCreate_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Bids", new[] { "UserCreate_Id" });
            DropColumn("dbo.Bids", "UserCreate_Id");
            DropColumn("dbo.Bids", "CheckedAt");
            DropColumn("dbo.Bids", "IsChecked");
        }
    }
}
