namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092403 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notice", "seq", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notice", "seq");
        }
    }
}
