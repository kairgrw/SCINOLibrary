namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr94 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pictures", "SourceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "SourceID", c => c.Int(nullable: false));
        }
    }
}
