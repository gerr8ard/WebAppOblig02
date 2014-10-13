using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BlogCentralVersion2.Models;

namespace BlogCentralVersion2.Controllers
{
    public class BlogAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BloggSentralenRepository repo;//skjer

        /// <summary>
        /// skjer
        /// </summary>
        public BlogAPIController()
        {
            repo = new BloggSentralenRepository();
        }

        // GET: api/BlogAPI
        [Route("api/BlogAPI")]
        public IQueryable<Object> GetBlogs()
        {
            var blogs = repo.GetAllBlogsAjax();
 
            return blogs;
        }
        /*
        // GET: api/BlogAPI
        [Route("api/BlogAPI/Comments/{BlogId:int}/{BlogPostId:int}")]
        public IQueryable<Object> GetComments(int BlogId, int BlogPostId)
        {
            var comments = repo.GetCommentsAjax(BlogId, BlogPostId);
            return comments;
        }*/

        // GET: api/BlogAPI
        [Route("api/BlogAPI/BlogPost/{BlogId:int}")]
        public IQueryable<Object> GetBlogPosts(int BlogId)
        {
            var blog = db.Blogs.Find(BlogId);
            var blogPosts = repo.GetAllBlogPostsAjax(BlogId);
            return blogPosts;
        }

        // GET: api/BlogAPI/5
        [ResponseType(typeof(Blog))]
        public IHttpActionResult GetBlog(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // PUT: api/BlogAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBlog(int id, Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            db.Entry(blog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BlogAPI
        [ResponseType(typeof(Blog))]
        public IHttpActionResult PostBlog(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Blogs.Add(blog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = blog.BlogId }, blog);
        }

        // DELETE: api/BlogAPI/5
        [ResponseType(typeof(Blog))]
        public IHttpActionResult DeleteBlog(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            db.Blogs.Remove(blog);
            db.SaveChanges();

            return Ok(blog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlogExists(int id)
        {
            return db.Blogs.Count(e => e.BlogId == id) > 0;
        }
    }
}