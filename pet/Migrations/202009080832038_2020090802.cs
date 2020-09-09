namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090802 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        orderseq = c.String(nullable: false, maxLength: 20),
                        companyseq = c.String(nullable: false, maxLength: 20),
                        companyname = c.String(nullable: false, maxLength: 50),
                        roomseq = c.String(nullable: false, maxLength: 20),
                        roomname = c.String(nullable: false, maxLength: 50),
                        country = c.String(maxLength: 10),
                        area = c.String(maxLength: 10),
                        address = c.String(maxLength: 100),
                        name = c.String(maxLength: 10),
                        tel = c.String(maxLength: 10),
                        pettype = c.String(maxLength: 20),
                        petsize = c.Int(),
                        petamount = c.Int(),
                        roomprice = c.Int(),
                        roomamount_amt = c.Int(),
                        medicine_infeed = c.Boolean(),
                        medicine_infeed_amt = c.Int(),
                        medicine_pill = c.Boolean(),
                        medicine_pill_amt = c.Int(),
                        medicine_paste = c.Boolean(),
                        medicine_paste_amt = c.Int(),
                        bath = c.Boolean(),
                        bath_amt = c.Int(),
                        hair = c.Boolean(),
                        hair_amt = c.Int(),
                        nails = c.Boolean(),
                        nails_amt = c.Int(),
                        state = c.Int(),
                        amt = c.Int(),
                        orderdates = c.DateTime(),
                        orderdatee = c.DateTime(),
                        memo = c.String(),
                        del_flag = c.String(nullable: false, maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.orderseq);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Order");
        }
    }
}
