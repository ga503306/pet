namespace pet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020090602 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Company", "state", c => c.Boolean());
            AlterColumn("dbo.Company", "morning", c => c.Boolean());
            AlterColumn("dbo.Company", "afternoon", c => c.Boolean());
            AlterColumn("dbo.Company", "night", c => c.Boolean());
            AlterColumn("dbo.Company", "midnight", c => c.Boolean());
            AlterColumn("dbo.Room", "pettype_cat", c => c.Boolean());
            AlterColumn("dbo.Room", "pettype_dog", c => c.Boolean());
            AlterColumn("dbo.Room", "pettype_other", c => c.Boolean());
            AlterColumn("dbo.Room", "canned", c => c.Boolean());
            AlterColumn("dbo.Room", "feed", c => c.Boolean());
            AlterColumn("dbo.Room", "catlitter", c => c.Boolean());
            AlterColumn("dbo.Room", "medicine_infeed", c => c.Boolean());
            AlterColumn("dbo.Room", "medicine_pill", c => c.Boolean());
            AlterColumn("dbo.Room", "medicine_paste", c => c.Boolean());
            AlterColumn("dbo.Room", "bath", c => c.Boolean());
            AlterColumn("dbo.Room", "hair", c => c.Boolean());
            AlterColumn("dbo.Room", "nails", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Room", "nails", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "hair", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "bath", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "medicine_paste", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "medicine_pill", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "medicine_infeed", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "catlitter", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "feed", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "canned", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "pettype_other", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "pettype_dog", c => c.String(maxLength: 5));
            AlterColumn("dbo.Room", "pettype_cat", c => c.String(maxLength: 5));
            AlterColumn("dbo.Company", "midnight", c => c.String(maxLength: 5));
            AlterColumn("dbo.Company", "night", c => c.String(maxLength: 5));
            AlterColumn("dbo.Company", "afternoon", c => c.String(maxLength: 5));
            AlterColumn("dbo.Company", "morning", c => c.String(maxLength: 5));
            AlterColumn("dbo.Company", "state", c => c.String(maxLength: 1));
        }
    }
}
