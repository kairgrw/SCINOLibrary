namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr93 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(nullable:true),
                        SourceID = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "Image_Id", c => c.Int());
            CreateIndex("dbo.Books", "Image_Id");
            AddForeignKey("dbo.Books", "Image_Id", "dbo.Pictures", "Id");
            DropColumn("dbo.Books", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Image", c => c.Binary());
            DropForeignKey("dbo.Books", "Image_Id", "dbo.Pictures");
            DropIndex("dbo.Books", new[] { "Image_Id" });
            DropColumn("dbo.Books", "Image_Id");
            DropTable("dbo.Pictures");
        }
    }
}
