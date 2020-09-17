namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091702 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionAnswer", "queseq", "dbo.Question");
            DropIndex("dbo.QuestionAnswer", new[] { "queseq" });
            DropTable("dbo.Question");
            DropTable("dbo.QuestionAnswer");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.ansseq);
            
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
            
            CreateIndex("dbo.QuestionAnswer", "queseq");
            AddForeignKey("dbo.QuestionAnswer", "queseq", "dbo.Question", "queseq");
        }
    }
}
