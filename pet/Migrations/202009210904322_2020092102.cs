namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092102 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Notice");
            AlterColumn("dbo.Notice", "noticeseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Notice", "noticeseq");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Notice");
            AlterColumn("dbo.Notice", "noticeseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Notice", "noticeseq");
        }
    }
}
