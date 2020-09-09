namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091001 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Room", "companyseq", "dbo.Company");
            DropPrimaryKey("dbo.Company");
            AlterColumn("dbo.Company", "companyseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Company", "companyseq");
            AddForeignKey("dbo.Room", "companyseq", "dbo.Company", "companyseq", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Room", "companyseq", "dbo.Company");
            DropPrimaryKey("dbo.Company");
            AlterColumn("dbo.Company", "companyseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Company", "companyseq");
            AddForeignKey("dbo.Room", "companyseq", "dbo.Company", "companyseq", cascadeDelete: true);
        }
    }
}
