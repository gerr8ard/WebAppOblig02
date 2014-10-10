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
    
    public class BlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IBloggSentralenRepository repo;

         public BlogsController()
        {
            repo = new BloggSentralenRepository();
        }

        public BlogsController(IBloggSentralenRepository _repo)
        {
            repo = _repo;
            
        }

        // GET: Blogs
        public ActionResult Index()
        {
            ViewBag.isLoggedIn = HttpContext.User.Identity.IsAuthenticated;//Sender med info til view'et om dersom bruker er innlogget
            var blogs = repo.getAllBlogs();
            return View(blogs);
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.isLoggedIn = HttpContext.User.Identity.IsAuthenticated;//Sender med info til view'et om dersom bruker er innlogget
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            ViewBag.userName = blog.OwnerOfBlog.UserName;//Sender med brukernavn for å sjekke om bruker er eier for et objekt
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }
    

        // GET: Blogs/Create
        [Authorize]
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogId,BlogTitle,BlogOwner,DateCreated")] Blog blog)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            blog.OwnerOfBlog = user;
                
            if (ModelState.IsValid)
            {
                
                blog.DateCreated = DateTime.Now;
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogId,BlogTitle,BlogOwner,DateCreated")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            //Sjekker om bruker har tilgang på objektet han forespør.
            if (User.Identity.Name != blog.OwnerOfBlog.UserName)
            {
                return View("NoAccess");
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
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
