using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    public class Product_storageController : Controller
    {
        private StoreContext db = new StoreContext();

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.account = User.Identity.Name;

            ViewBag.id_product = new SelectList(db.Products, "id_product", "name_product");

            var allstorages = await db.Product_storage.Include(p => p.product).ToListAsync();

            return View(allstorages);
        }

        [Authorize(Roles = "admin")]
        private async Task<ActionResult> Create()
        {
            var allprocurements = await db.Procurements.ToListAsync();
            var allstorages = await db.Product_storage.Include(p => p.product).ToListAsync();

            foreach (Procurement item in allprocurements)
            {
                foreach (Product_storage storage in allstorages)
                {
                    if (db.Product_storage.Find(item.id_product) == null)
                    {
                        storage.id_product = item.id_product;
                        storage.count = item.count_procurement;
                    }
                    else
                    {
                        storage.id_product = db.Product_storage.Find(item.id_product).id_product;
                        storage.count = db.Product_storage.Find(item.id_product).count + item.count_procurement;
                    }
                }
            }

            foreach (Product_storage storage in allstorages)
            {
                db.Product_storage.Add(storage);
            }
            db.SaveChanges();

            return View(allstorages);
        }

        [Authorize(Roles = "admin")]
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
