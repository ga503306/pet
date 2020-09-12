namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091202 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderCancel",
                c => new
                    {
                        ocseq = c.String(nullable: false, maxLength: 20),
                        orderseq = c.String(maxLength: 20),
                        memo = c.String(),
                        del_flag = c.String(maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.ocseq)
                .ForeignKey("dbo.Order", t => t.orderseq)
                .Index(t => t.orderseq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderCancel", "orderseq", "dbo.Order");
            DropIndex("dbo.OrderCancel", new[] { "orderseq" });
            DropTable("dbo.OrderCancel");
        }
    }
}
