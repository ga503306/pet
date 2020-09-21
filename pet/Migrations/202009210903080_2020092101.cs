namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092101 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notice",
                c => new
                    {
                        noticeseq = c.String(nullable: false, maxLength: 20),
                        fromseq = c.String(maxLength: 20),
                        toseq = c.String(maxLength: 20),
                        text = c.String(),
                        state = c.Boolean(),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.noticeseq);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notice");
        }
    }
}
