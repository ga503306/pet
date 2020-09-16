namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202009140 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Evalution");
            AlterColumn("dbo.Evalution", "evaseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Evalution", "evaseq");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Evalution");
            AlterColumn("dbo.Evalution", "evaseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Evalution", "evaseq");
        }
    }
}
