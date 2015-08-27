namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr96 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "IsBidCreated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IsBidCreated");
        }
    }
}
