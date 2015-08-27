namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "Placement", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Books", "Image_Id", c => c.Int());
            CreateIndex("dbo.Books", "Image_Id");
            AddForeignKey("dbo.Books", "Image_Id", "dbo.Pictures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Image_Id", "dbo.Pictures");
            DropIndex("dbo.Books", new[] { "Image_Id" });
            DropColumn("dbo.Books", "Image_Id");
            DropColumn("dbo.Books", "Placement");
            DropTable("dbo.Pictures");
        }
    }
}
