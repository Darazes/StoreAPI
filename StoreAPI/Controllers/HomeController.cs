using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreAPI.Models;
using System.Data.Entity;

namespace StoreAPI.Controllers
{
    public class HomeController : Controller
    {
        StoreContext db = new StoreContext(); 

        public ActionResult Index()
        {
            ViewBag.account = User.Identity.Name;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}