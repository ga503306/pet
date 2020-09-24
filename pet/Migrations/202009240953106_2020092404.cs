namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020092404 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Signalr",
                c => new
                    {
                        signalrseq = c.Int(nullable: false, identity: true),
                        whoseq = c.String(maxLength: 20),
                        connectid = c.String(),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.signalrseq);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Signalr");
        }
    }
}
