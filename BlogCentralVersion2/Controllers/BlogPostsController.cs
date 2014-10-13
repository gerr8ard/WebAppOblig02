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
        public ActionResult Index(int BlogId)
        {
            ViewBag.isLoggedIn = HttpContext.User.Identity.IsAuthenticated;//Sender med info til view'et om dersom bruker er innlogget
            //ViewBag.user = HttpContext.User.Identity.Name;

            var blog = db.Blogs.Find(BlogId);
            ViewBag.userName = blog.OwnerOfBlog.UserName;//Sender med brukernavn for å sjekke om bruker er eier for et objekt
            ViewBag.BlogId = BlogId;

            //var blogPost = repo.getAllBlogPosts(BlogId);
            return View("IndexAjax");
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(int? BlogPostId, int BlogId)
        {
            ViewBag.isLoggedIn = HttpContext.User.Identity.IsAuthenticated;//Sender med info til view'et om dersom bruker er innlogget
            ViewBag.BlogId = BlogId;
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
        public ActionResult Create(int BlogId)
        {
            ViewBag.BlogId = BlogId;
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogPostId,BlogPostTitle,BlogPostAuthor,BlogPostPost,DateCreated, isOpen")] BlogPost blogPost, int BlogId)
        {

            var user = db.Users.Find(User.Identity.GetUserId());
            blogPost.OwnerOfBlogPost = user;
            var username = blogPost.OwnerOfBlogPost.CommentUserName;//Henter ut brukernavnet til brukeren.
            if (ModelState.IsValid)
            {
                
                Blog blog = db.Blogs.Find(BlogId);
                blogPost.BlogPostAuthor = username;//Setter forfatter av innlegg automatisk.
                blogPost.DateCreated = DateTime.Now;
                blogPost.Blog = blog;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index", new {BlogId = BlogId});
            }

           
            return View(blogPost);
        }

        [Authorize]
        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? BlogPostId, int BlogId)
        {
            ViewBag.BlogId = BlogId;
            if (BlogPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            //Sjekker om bruker har tilgang på objektet han forespør.
            if (User.Identity.Name != blogPost.OwnerOfBlogPost.UserName)
            {
                return View("NoAccess");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", blogPost.Blog.BlogId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogPostId,BlogPostTitle,BlogPostAuthor,BlogPostPost,DateCreated,isOpen")] BlogPost blogPost, int BlogId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { BlogId = BlogId });
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", blogPost.Blog.BlogId);
            return View(blogPost);
        }

        [Authorize]
        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? BlogPostId, int BlogId)
        {
            ViewBag.BlogId = BlogId;
            if (BlogPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            //Sjekker om bruker har tilgang på objektet han forespør.
            if (User.Identity.Name != blogPost.OwnerOfBlogPost.UserName)
            {
                return View("NoAccess");
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int BlogPostId, int BlogId)
        {
            BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index", new { BlogId = BlogId });
        }

        [Authorize]
        public ActionResult Comment(int BlogPostId, int BlogId)
        {
            ViewBag.BlogPostId = BlogPostId;
            ViewBag.BlogId = BlogId;
            var comment = db.Comments.Where(x => x.BlogPost.BlogPostId == BlogPostId).ToList<Comment>();
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "CommentId,CommentName,CommentPost,DateCreated")] Comment comment, int BlogId, int BlogPostId)
        {

            ViewBag.BlogId = BlogId;
            ViewBag.BlogPostId = BlogPostId;
            var user = db.Users.Find(User.Identity.GetUserId());
            comment.OwnerOfComment = user;
            var username = user.CommentUserName;//Henter ut brukernavnet til brukeren.

            if (ModelState.IsValid)
            {

                Blog blog = db.Blogs.Find(BlogId);
                comment.CommentName = username;
                comment.Datecreated = DateTime.Now;
                BlogPost blogPost = db.BlogPosts.Find(BlogPostId);
                comment.BlogPost = blogPost;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "BlogPosts" , new { BlogId = BlogId, BlogPostId = BlogPostId });
            }


            return View();
        }


        [Authorize]
        public ActionResult DeleteComment(int BlogId, int BlogPostId, int CommentId)
        {
            Blog blog = db.Blogs.Find(BlogId);
            ViewBag.userName = blog.OwnerOfBlog.UserName;//Sender med brukernavn for å sjekke om bruker er eier for et objekt
            ViewBag.BlogId = BlogId;
            ViewBag.BlogPostId = BlogPostId;
            repo.DeleteComment(CommentId);
            return RedirectToAction("Details",  "BlogPosts" , new { BlogId = BlogId, BlogPostId = BlogPostId });
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
