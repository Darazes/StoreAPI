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

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.account = User.Identity.Name;

            return View();
        }
    }

}