namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091703 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        queseq = c.String(nullable: false, maxLength: 20),
                        roomseq = c.String(maxLength: 20),
                        companyseq = c.String(maxLength: 20),
                        message = c.String(),
                        del_flag = c.String(maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.queseq);
            
            CreateTable(
                "dbo.QuestionAnswer",
                c => new
                    {
                        ansseq = c.String(nullable: false, maxLength: 20),
                        queseq = c.String(maxLength: 20),
                        memberseq = c.String(maxLength: 20),
                        companyseq = c.String(maxLength: 20),
                        message = c.String(),
                        del_flag = c.String(maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.ansseq)
                .ForeignKey("dbo.Question", t => t.queseq)
                .Index(t => t.queseq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionAnswer", "queseq", "dbo.Question");
            DropIndex("dbo.QuestionAnswer", new[] { "queseq" });
            DropTable("dbo.QuestionAnswer");
            DropTable("dbo.Question");
        }
    }
}
