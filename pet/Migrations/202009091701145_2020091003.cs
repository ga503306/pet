namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091003 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Order", "orderseq");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Order", "orderseq");
        }
    }
}
