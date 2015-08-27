namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
        }
    }
}
