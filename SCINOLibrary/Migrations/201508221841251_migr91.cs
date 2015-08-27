namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr91 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pictures", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "Name", c => c.String());
        }
    }
}
