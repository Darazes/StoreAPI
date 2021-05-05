using System.Web.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    public class HomeController : Controller
    {
        StoreContext db = new StoreContext();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            ViewBag.account = User.Identity.Name;

            return View();
        }
    }

}