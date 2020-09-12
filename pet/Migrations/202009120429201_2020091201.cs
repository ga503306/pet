namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020091201 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Order");
            CreateTable(
                "dbo.Evalution",
                c => new
                    {
                        evaseq = c.String(nullable: false, maxLength: 20),
                        orderseq = c.String(maxLength: 20),
                        star = c.Int(),
                        memo = c.String(),
                        del_flag = c.String(maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.evaseq)
                .ForeignKey("dbo.Order", t => t.orderseq)
                .Index(t => t.orderseq);
            
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Order", "orderseq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Evalution", "orderseq", "dbo.Order");
            DropIndex("dbo.Evalution", new[] { "orderseq" });
            DropPrimaryKey("dbo.Order");
            AlterColumn("dbo.Order", "orderseq", c => c.String(nullable: false, maxLength: 20));
            DropTable("dbo.Evalution");
            AddPrimaryKey("dbo.Order", "orderseq");
        }
    }
}
