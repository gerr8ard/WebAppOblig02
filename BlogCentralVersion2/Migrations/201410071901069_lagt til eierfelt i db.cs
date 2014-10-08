namespace BlogCentralVersion2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lagttileierfeltidb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "OwnerOfBlogPost_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Blogs", "OwnerOfBlog_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Comments", "OwnerOfComment_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.BlogPosts", "OwnerOfBlogPost_Id");
            CreateIndex("dbo.Blogs", "OwnerOfBlog_Id");
            CreateIndex("dbo.Comments", "OwnerOfComment_Id");
            AddForeignKey("dbo.Blogs", "OwnerOfBlog_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comments", "OwnerOfComment_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.BlogPosts", "OwnerOfBlogPost_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPosts", "OwnerOfBlogPost_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "OwnerOfComment_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Blogs", "OwnerOfBlog_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "OwnerOfComment_Id" });
            DropIndex("dbo.Blogs", new[] { "OwnerOfBlog_Id" });
            DropIndex("dbo.BlogPosts", new[] { "OwnerOfBlogPost_Id" });
            DropColumn("dbo.Comments", "OwnerOfComment_Id");
            DropColumn("dbo.Blogs", "OwnerOfBlog_Id");
            DropColumn("dbo.BlogPosts", "OwnerOfBlogPost_Id");
        }
    }
}
