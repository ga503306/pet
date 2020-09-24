namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092402 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notice", "type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notice", "type", c => c.String());
        }
    }
}
