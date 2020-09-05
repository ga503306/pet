namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090603 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Room", "img1", c => c.String());
            AddColumn("dbo.Room", "img2", c => c.String());
            AddColumn("dbo.Room", "img3", c => c.String());
            AddColumn("dbo.Room", "img4", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Room", "img4");
            DropColumn("dbo.Room", "img3");
            DropColumn("dbo.Room", "img2");
            DropColumn("dbo.Room", "img1");
        }
    }
}
