namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091701 : DbMigration
    {
        public override void Up()
        {
          
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
