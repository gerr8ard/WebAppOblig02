using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogCentralVersion2.Models;
using BlogCentralVersion2.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BlogCentralVersion2.Tests
{
    [TestClass]
    public class Tests
    {
        Mock<IBloggSentralenRepository> _repo;

        [TestInitialize]
        public void SetupContext()
        {
            _repo = new Mock<IBloggSentralenRepository>();
        }
        /// <summary>
        /// Test for blogger i index metoden 
        /// </summary>
        [TestMethod]
        public void getAllBlogs()
        {

            List<Blog> fakeBlogs = new List<Blog>{
                new Blog { BlogOwner = "test1", BlogTitle = "test1"},
                new Blog { BlogOwner = "test2", BlogTitle = " test2"}
            };

            _repo.Setup(x => x.getAllBlogs()).Returns(fakeBlogs);
            var controller = new BlogsController(_repo.Object);

            var result = controller.Index() as ViewResult;

            CollectionAssert.AllItemsAreInstancesOfType((ICollection)result.ViewData.Model, typeof(Blog));
            Assert.IsNotNull(result, "View result is null.");
            var op = result.ViewData.Model as List<Blog>;
            Assert.AreEqual(2, op.Count, "Got wrong number of Blogs");
        }

        [TestMethod]
        public void getAllBlogPosts()
        {

            List<BlogPost> fakeBlogPosts = new List<BlogPost>{
                new BlogPost {BlogPostAuthor = "Arthur", BlogPostTitle = "Arthur på tur", BlogPostPost = "Arthur dro på en tur"},
                new BlogPost {BlogPostAuthor = "Kåre", BlogPostTitle = "Kåre på tur", BlogPostPost = "Kåre dro på en tur"},
                new BlogPost {BlogPostAuthor = "Jenny", BlogPostTitle = "Jenny på tur", BlogPostPost = "Jenny dro på en tur"}
            };

            _repo.Setup(x => x.getAllBlogPosts(1)).Returns(fakeBlogPosts);
            var controller = new BlogPostsController(_repo.Object);

            var result = controller.Index(1) as ViewResult;

            CollectionAssert.AllItemsAreInstancesOfType((ICollection)result.ViewData.Model, typeof(BlogPost));
            Assert.IsNotNull(result, "View result is null.");
            var op = result.ViewData.Model as List<BlogPost>;
            Assert.AreEqual(3, op.Count, "Got wrong number of BlogPosts");

        }
        /// <summary>
        /// Test som sjekker om et innlegg er åpen for kommentarer.
        /// Har ikke fått den til å virke, men jeg mener det ikke skal være så langt unna
        /// </summary>
        [TestMethod]
        public void testCommentTrue()
        {

            BlogPost blogPost = new BlogPost{
                isOpen = true
            };

            Blog blog = new Blog
            {
                BlogTitle = "Test blogg"
            };

            _repo.Setup(x => x.getBlogpost(1)).Returns(blogPost);
            _repo.Setup(x => x.getBlog(1)).Returns(blog);
            var controller = new BlogPostsController(_repo.Object);

            var result = controller.Details(1, 1) as ViewResult;
            var viewPost = (BlogPost)result.ViewData.Model;

            Assert.AreEqual(true, viewPost.isOpen);
        }
        /// <summary>
        /// Test som skal sjekke om et innlegg er stengt for kommentar.
        /// Samme som over. Funker ikke, men ikke langt fra.
        /// </summary>
        [TestMethod]
        public void testCommentFalse()
        {

            BlogPost blogPost = new BlogPost
            {
                isOpen = false
            };

            Blog blog = new Blog
            {
                BlogTitle = "Test blogg"
            };

            _repo.Setup(x => x.getBlogpost(1)).Returns(blogPost);
            _repo.Setup(x => x.getBlog(1)).Returns(blog);
            var controller = new BlogPostsController(_repo.Object);

            var result = controller.Details(1, 1) as ViewResult;
            var viewPost = (BlogPost)result.ViewData.Model;

            Assert.AreEqual(false, viewPost.isOpen);
        }
    }
}
