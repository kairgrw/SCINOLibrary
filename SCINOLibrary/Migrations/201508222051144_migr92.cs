namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr92 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Image_Id", "dbo.Pictures");
            DropIndex("dbo.Books", new[] { "Image_Id" });
            AddColumn("dbo.Books", "Image", c => c.Binary());
            DropColumn("dbo.Books", "Image_Id");
            DropTable("dbo.Pictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "Image_Id", c => c.Int());
            DropColumn("dbo.Books", "Image");
            CreateIndex("dbo.Books", "Image_Id");
            AddForeignKey("dbo.Books", "Image_Id", "dbo.Pictures", "Id");
        }
    }
}
