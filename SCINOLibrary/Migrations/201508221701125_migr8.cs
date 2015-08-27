namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Created", c => c.DateTime(nullable:true));
            AddColumn("dbo.Books", "Changed", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Changed");
            DropColumn("dbo.Books", "Created");
        }
    }
}
