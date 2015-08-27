namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Created", c => c.DateTime());
        }
    }
}
