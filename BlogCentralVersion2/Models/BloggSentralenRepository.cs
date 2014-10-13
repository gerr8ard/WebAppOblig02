using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public class BloggSentralenRepository : IBloggSentralenRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Sletter valgt kommentar fra databasen.
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteComment(int? id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        /// <summary>
        /// Henter alle blogger fra databasen
        /// </summary>
        /// <returns>En oversikt over alle blogger i databasen</returns>
        public IEnumerable<Blog> getAllBlogs()
        {
            return db.Blogs;
        }

        /// <summary>
        /// Henter ut alle innlegg i en blogg vha en BlogId.
        /// </summary>
        /// <param name="BlogId"></param>
        /// <returns>En oversikt over alle innlegg tilknyttet en gitt blogg vha BlogId</returns>
        public ICollection<BlogPost> getAllBlogPosts(int? BlogId)
        {
            return db.BlogPosts
                .Where(x => x.Blog.BlogId == BlogId)
                .OrderByDescending(x => x.BlogPostId)
                .ToList<BlogPost>();
        }
        
        /// <summary>
        /// Henter ut en gitt blogg vha BlogId. Brukes i forbindelse med testing.
        /// </summary>
        /// <param name="BlogId"></param>
        /// <returns></returns>
        public Blog getBlog(int? BlogId)
        {
            var blog = db.Blogs.Find(BlogId);
            return blog;
        }

        /// <summary>
        /// Henter ut et gitt innlegg vha BlogId. Brukes i forbindelse med testing.
        /// </summary>
        /// <param name="BlogId"></param>
        /// <returns></returns>
        public BlogPost getBlogpost(int? BlogId)
        {
            return db.BlogPosts
                .Include("isOpen")
                .Where(x => x.BlogPostId == BlogId)
                .FirstOrDefault();
        }
        /// <summary>
        /// Henter ut alle blogger.
        /// </summary>
        /// <returns>Oversikt over alle blogger</returns>
        public IQueryable<Object> GetAllBlogsAjax()
        {
            var blogg = db.Blogs.Include("ApplicationUser");

            var collection = blogg.Select(x => new
            {
                id = x.BlogId,
                title = x.BlogTitle,
                created = x.DateCreated,
                user = x.OwnerOfBlog.UserName
            });
            return collection;
        }

        public IQueryable<Object> GetAllBlogPostsAjax(int BlogId)
        {
            var blogPosts = db.BlogPosts.Include("ApplicationUser")
                .Where(x => x.Blog.BlogId == BlogId);

            var collection = blogPosts.Select(x => new
            {
                blogId = x.Blog.BlogId,
                blogPostId = x.BlogPostId,
                title = x.BlogPostTitle,
                blogpost = x.BlogPostPost,
                created = x.DateCreated,
                user = x.OwnerOfBlogPost.UserName
            });
            return collection;
        }

        public IQueryable<Object> GetCommentsAjax(int BlogId, int BlogPostId)
        {
            var comments = db.Comments.Include("ApplicationUser")
                .Where(x => x.BlogPost.BlogPostId == BlogPostId);

            var collection = comments.Select(x => new
            {
                blogId = x.BlogPost.Blog.BlogId,
                blogPostId = x.BlogPost.BlogPostId,
                comment = x.CommentPost,
                commentUser = x.CommentName,
                created = x.Datecreated,
                user = x.OwnerOfComment.UserName
            });
            return collection;
        }
    }
}