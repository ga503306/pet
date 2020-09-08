namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090801 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Room", "canned", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Room", "feed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Room", "catlitter", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Room", "visit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Room", "visit", c => c.Int());
            AlterColumn("dbo.Room", "catlitter", c => c.Boolean());
            AlterColumn("dbo.Room", "feed", c => c.Boolean());
            AlterColumn("dbo.Room", "canned", c => c.Boolean());
        }
    }
}
