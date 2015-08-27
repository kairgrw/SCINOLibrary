namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        OnExchange = c.Boolean(nullable: false),
                        Price = c.Double(nullable: false),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GenreBooks",
                c => new
                    {
                        Genre_ID = c.Int(nullable: false),
                        Book_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_ID, t.Book_ID })
                .ForeignKey("dbo.Genres", t => t.Genre_ID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_ID, cascadeDelete: true)
                .Index(t => t.Genre_ID)
                .Index(t => t.Book_ID);
            CreateTable(
                "dbo.UserBooks",
                c => new
                {
                    Book_ID = c.Int(nullable: false),
                    User_ID = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.Book_ID, t.User_ID })
                .ForeignKey("dbo.Books", t => t.Book_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.Book_ID)
                .Index(t => t.User_ID);
            
        }
        
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GenreBooks", "Book_ID", "dbo.Books");
            DropForeignKey("dbo.GenreBooks", "Genre_ID", "dbo.Genres");
            DropIndex("dbo.GenreBooks", new[] { "Book_ID" });
            DropIndex("dbo.GenreBooks", new[] { "Genre_ID" });
            DropIndex("dbo.Books", new[] { "Owner_Id" });
            DropTable("dbo.GenreBooks");
            DropTable("dbo.Genres");
            DropTable("dbo.Books");
        }
    }
}
