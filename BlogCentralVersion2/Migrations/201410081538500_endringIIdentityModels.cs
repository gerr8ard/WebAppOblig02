namespace BlogCentralVersion2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class endringIIdentityModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CommentUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CommentUserName");
        }
    }
}
