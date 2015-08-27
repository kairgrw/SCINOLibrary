namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBooks",
                c => new
                {
                    User_ID = c.String(maxLength: 128, nullable: false),
                    Book_ID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.User_ID, t.Book_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Book_ID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBooks", "Book_ID", "dbo.Books");
            DropForeignKey("dbo.UserBooks", "User_ID", "dbo.AspNetUsers");
            DropIndex("dbo.UserBooks", new[] { "Book_ID" });
            DropIndex("dbo.UserBooks", new[] { "User_ID" });
            DropTable("dbo.UserBooks");
        }
    }
}
