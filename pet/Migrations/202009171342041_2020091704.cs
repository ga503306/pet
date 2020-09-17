namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091704 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "memberseq", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "memberseq");
        }
    }
}
