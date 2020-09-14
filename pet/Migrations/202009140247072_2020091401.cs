namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091401 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderCancel", "reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderCancel", "reason");
        }
    }
}
