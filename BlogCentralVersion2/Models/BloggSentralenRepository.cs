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
        /// Henter ut alle innlegg i en blogg vha en BloggId.
        /// </summary>
        /// <param name="BloggId"></param>
        /// <returns>En oversikt over alle innlegg tilknyttet en gitt blogg vha BloggId</returns>
        public ICollection<BlogPost> getAllBlogPosts(int? BloggId)
        {
            return db.BlogPosts
                .Where(x => x.Blog.BlogId == BloggId)
                .OrderByDescending(x => x.BlogPostId)
                .ToList<BlogPost>();
        }
        
        /// <summary>
        /// Henter ut en gitt blogg vha BloggId. Brukes i forbindelse med testing.
        /// </summary>
        /// <param name="BloggId"></param>
        /// <returns></returns>
        public Blog getBlog(int? BloggId)
        {
            var blog = db.Blogs.Find(BloggId);
            return blog;
        }

        /// <summary>
        /// Henter ut et gitt innlegg vha BloggId. Brukes i forbindelse med testing.
        /// </summary>
        /// <param name="BloggId"></param>
        /// <returns></returns>
        public BlogPost getBlogpost(int? BloggId)
        {
            return db.BlogPosts
                .Include("isOpen")
                .Where(x => x.BlogPostId == BloggId)
                .FirstOrDefault();
        }
    }
}