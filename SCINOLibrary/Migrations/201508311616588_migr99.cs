namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr99 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bids", "IsChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bids", "IsChecked");
        }
    }
}
