namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092401 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notice", "type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notice", "type");
        }
    }
}
