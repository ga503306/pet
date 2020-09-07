namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090902 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Room", "state", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Room", "state", c => c.Boolean(nullable: false));
        }
    }
}
