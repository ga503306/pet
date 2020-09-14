namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091402 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Evalution", "orderseq", "dbo.Order");
            DropForeignKey("dbo.OrderCancel", "orderseq", "dbo.Order");
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.OrderCancel");
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.OrderCancel", "ocseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Order", "orderseq");
            AddPrimaryKey("dbo.OrderCancel", "ocseq");
            AddForeignKey("dbo.Evalution", "orderseq", "dbo.Order", "orderseq");
            AddForeignKey("dbo.OrderCancel", "orderseq", "dbo.Order", "orderseq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCancel", "orderseq", "dbo.Order");
            DropForeignKey("dbo.Evalution", "orderseq", "dbo.Order");
            DropPrimaryKey("dbo.OrderCancel");
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.OrderCancel", "ocseq", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.OrderCancel", "ocseq");
            AddPrimaryKey("dbo.Order", "orderseq");
            AddForeignKey("dbo.OrderCancel", "orderseq", "dbo.Order", "orderseq");
            AddForeignKey("dbo.Evalution", "orderseq", "dbo.Order", "orderseq");
        }
    }
}
