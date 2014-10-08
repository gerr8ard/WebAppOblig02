using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BlogCentralVersion2.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private string name = "Pål Gerrard Gaare-Skogsrud";
        public ActionResult Index()
        {
            ViewBag.isLoggedIn = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = name;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = name;

            return View();
        }
    }
}