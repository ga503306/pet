namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091002 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Room");
            DropPrimaryKey("dbo.Member");
            AlterColumn("dbo.Room", "roomseq", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Member", "memberseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Room", "roomseq");
            AddPrimaryKey("dbo.Member", "memberseq");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Member");
            DropPrimaryKey("dbo.Room");
            AlterColumn("dbo.Member", "memberseq", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Room", "roomseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Member", "memberseq");
            AddPrimaryKey("dbo.Room", "roomseq");
        }
    }
}
