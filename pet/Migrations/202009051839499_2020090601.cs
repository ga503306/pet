namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090601 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        companyseq = c.String(nullable: false, maxLength: 20),
                        companyname = c.String(nullable: false, maxLength: 50),
                        companybrand = c.String(nullable: false, maxLength: 50),
                        phone = c.String(nullable: false, maxLength: 10),
                        email = c.String(nullable: false, maxLength: 200),
                        pwd = c.String(nullable: false, maxLength: 50),
                        pwdsalt = c.String(nullable: false, maxLength: 200),
                        country = c.String(maxLength: 10),
                        area = c.String(maxLength: 10),
                        address = c.String(maxLength: 100),
                        pblicense = c.String(maxLength: 20),
                        effectivedate = c.DateTime(),
                        state = c.String(maxLength: 1),
                        avatar = c.String(),
                        bannerimg = c.String(),
                        del_flag = c.String(maxLength: 1),
                        introduce = c.String(),
                        morning = c.String(maxLength: 5),
                        afternoon = c.String(maxLength: 5),
                        night = c.String(maxLength: 5),
                        midnight = c.String(maxLength: 5),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.companyseq);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        roomseq = c.String(nullable: false, maxLength: 20),
                        roomname = c.String(maxLength: 50),
                        companyseq = c.String(nullable: false, maxLength: 20),
                        introduce = c.String(),
                        pettype_cat = c.String(maxLength: 5),
                        pettype_dog = c.String(maxLength: 5),
                        pettype_other = c.String(maxLength: 5),
                        petsizes = c.Int(),
                        petsizee = c.Int(),
                        roomamount = c.Int(),
                        roomprice = c.Int(),
                        roomamount_amt = c.Int(),
                        walk = c.Int(),
                        canned = c.String(maxLength: 5),
                        feed = c.String(maxLength: 5),
                        catlitter = c.String(maxLength: 5),
                        visit = c.Int(),
                        medicine_infeed = c.String(maxLength: 5),
                        medicine_infeed_amt = c.Int(),
                        medicine_pill = c.String(maxLength: 5),
                        medicine_pill_amt = c.Int(),
                        medicine_paste = c.String(maxLength: 5),
                        medicine_paste_amt = c.Int(),
                        bath = c.String(maxLength: 5),
                        bath_amt = c.Int(),
                        hair = c.String(maxLength: 5),
                        hair_amt = c.Int(),
                        nails = c.String(maxLength: 5),
                        nails_amt = c.Int(),
                        state = c.Int(nullable: false),
                        del_flag = c.String(nullable: false, maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.roomseq)
                .ForeignKey("dbo.Company", t => t.companyseq, cascadeDelete: true)
                .Index(t => t.companyseq);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        memberseq = c.String(nullable: false, maxLength: 20),
                        membername = c.String(nullable: false, maxLength: 20),
                        phone = c.String(maxLength: 20),
                        email = c.String(nullable: false, maxLength: 200),
                        pwd = c.String(nullable: false, maxLength: 50),
                        pwdsalt = c.String(nullable: false, maxLength: 200),
                        avatar = c.String(),
                        del_flag = c.String(maxLength: 1),
                        postseq = c.String(maxLength: 20),
                        postname = c.String(maxLength: 20),
                        postday = c.DateTime(),
                        updateseq = c.String(maxLength: 20),
                        updatename = c.String(maxLength: 20),
                        updateday = c.DateTime(),
                    })
                .PrimaryKey(t => t.memberseq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Room", "companyseq", "dbo.Company");
            DropIndex("dbo.Room", new[] { "companyseq" });
            DropTable("dbo.Member");
            DropTable("dbo.Room");
            DropTable("dbo.Company");
        }
    }
}
