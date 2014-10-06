using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCentralVersion2.Controllers
{
    public class HomeController : Controller
    {
        private string name = "Pål Gerrard Gaare-Skogsrud";
        public ActionResult Index()
        {
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