namespace BlogCentralVersion2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fjernetReqIComment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "CommentName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "CommentName", c => c.String(nullable: false));
        }
    }
}
