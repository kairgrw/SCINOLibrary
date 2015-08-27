namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "PublishYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "PublishYear");
        }
    }
}
