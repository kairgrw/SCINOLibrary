namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr98 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bids", "IsChecked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bids", "IsChecked", c => c.Boolean(nullable: false));
        }
    }
}
