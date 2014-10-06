using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public class BloggSentralenRepository : IBloggSentralenRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void DeleteComment(int? id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        public IEnumerable<Blog> getAllBlogs()
        {
            return db.Blogs;
        }

        public ICollection<BlogPost> getAllBlogPosts(int? BloggId)
        {
            return db.BlogPosts
                .Where(x => x.Blog.BlogId == BloggId)
                .OrderByDescending(x => x.BlogPostId)
                .ToList<BlogPost>();
        }
        
        public Blog getBlog(int? BloggId)
        {
            var blog = db.Blogs.Find(BloggId);
            return blog;
        }

        public BlogPost getBlogpost(int? BloggId)
        {
            return db.BlogPosts
                .Include("isOpen")
                .Where(x => x.BlogPostId == BloggId)
                .FirstOrDefault();
        }
    }
}