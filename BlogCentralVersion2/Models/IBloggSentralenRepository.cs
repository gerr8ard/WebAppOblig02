using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public interface IBloggSentralenRepository
    {

        void DeleteComment(int? id);
        IEnumerable<Blog> getAllBlogs();
        ICollection<BlogPost> getAllBlogPosts(int? BlogId);
        Blog getBlog(int? BlogId);
        BlogPost getBlogpost(int? BlogId);
        IQueryable<Object> GetAllBlogsAjax();
        IQueryable<Object> GetAllBlogPostsAjax(int BlogId);
    }
}