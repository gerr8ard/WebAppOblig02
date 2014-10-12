namespace BlogCentralVersion2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lagtTilReqPåBlogPost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPosts", "BlogPostTitle", c => c.String(nullable: false));
            AlterColumn("dbo.BlogPosts", "BlogPostPost", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogPosts", "BlogPostPost", c => c.String());
            AlterColumn("dbo.BlogPosts", "BlogPostTitle", c => c.String());
        }
    }
}
