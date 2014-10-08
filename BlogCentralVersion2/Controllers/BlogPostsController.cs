using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogCentralVersion2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BlogCentralVersion2.Controllers
{
    public class BlogPostsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private IBloggSentralenRepository repo;
        public BlogPostsController()
        {
            repo = new BloggSentralenRepository();
        }

        public BlogPostsController(IBloggSentralenRepository _repo)
        {
            repo = _repo;
            
            
        }
        // GET: BlogPosts
        public ActionResult Index(int BloggId)
        {
            ViewBag.BloggId = BloggId;
            var blogPost = repo.getAllBlogPosts(BloggId);
            return View(blogPost);
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(int? BlogPostId, int BloggId)
        {
            ViewBag.BloggId = BloggId;
            ViewBag.BlogPostId = BlogPostId;
            if (BlogPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        [Authorize]
        // GET: BlogPosts/Create
        public ActionResult Create(int BloggId)
        {
            ViewBag.BloggId = BloggId;
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogPostId,BlogPostTitle,BlogPostAuthor,BlogPostPost,DateCreated, isOpen")] BlogPost blogPost, int BloggId)
        {

            var user = db.Users.Find(User.Identity.GetUserId());
            blogPost.OwnerOfBlogPost = user;

            if (ModelState.IsValid)
            {
                
                Blog blog = db.Blogs.Find(BloggId);
                blogPost.DateCreated = DateTime.Now;
                blogPost.Blog = blog;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index", new {BloggId = BloggId});
            }

           
            return View(blogPost);
        }

        [Authorize]
        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? BlogPostId, int BloggId)
        {
            ViewBag.BloggId = BloggId;
            if (BlogPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", blogPost.Blog.BlogId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogPostId,BlogPostTitle,BlogPostAuthor,BlogPostPost,DateCreated,isOpen")] BlogPost blogPost, int BloggId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { BloggId = BloggId });
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", blogPost.Blog.BlogId);
            return View(blogPost);
        }

        [Authorize]
        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? BlogPostId, int BloggId)
        {
            ViewBag.BloggId = BloggId;
            if (BlogPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int BlogPostId, int BloggId)
        {
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index", new { BloggId = BloggId });
        }

        [Authorize]
        public ActionResult Comment(int BlogPostId, int BloggId)
        {
            ViewBag.BlogPostId = BlogPostId;
            ViewBag.BloggId = BloggId;
            var comment = db.Comments.Where(x => x.BlogPost.BlogPostId == BlogPostId).ToList<Comment>();
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "CommentId,CommentName,CommentPost,DateCreated")] Comment comment, int BloggId, int BlogPostId)
        {

            ViewBag.BloggId = BloggId;
            ViewBag.BlogPostId = BlogPostId;
            var user = db.Users.Find(User.Identity.GetUserId());
            comment.OwnerOfComment = user;

            if (ModelState.IsValid)
            {

                Blog blog = db.Blogs.Find(BloggId);
                comment.Datecreated = DateTime.Now;
                BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
                comment.BlogPost = blogPost;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "BlogPosts" , new { BloggId = BloggId, BlogPostId = BlogPostId });
            }


            return View();
        }


        [Authorize]
        public ActionResult DeleteComment(int BloggId, int BlogPostId, int CommentId)
        {
            ViewBag.BloggId = BloggId;
            ViewBag.BlogPostId = BlogPostId;
            repo.DeleteComment(CommentId);
            return RedirectToAction("Details",  "BlogPosts" , new { BloggId = BloggId, BlogPostId = BlogPostId });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
