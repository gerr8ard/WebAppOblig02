namespace BlogCentralVersion2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fjernetlitt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "BlogOwner", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogs", "BlogOwner", c => c.String(nullable: false));
        }
    }
}
