using Newtonsoft.Json;
using StoreAPI.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreAPI.Controllers
{
    public class ProductController : Controller
    {

        StoreContext db = new StoreContext();

        public ActionResult Index()
        {
            var products = db.Products.Include(c => c.category);

            return View(products.ToList());
        }

        public string IndexJson()
        {
            List<Product> products = db.Products.ToList();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        public ActionResult Search(string searching)
        {
            return View(db.Products.Where(n => n.name_product.Contains(searching) || searching == null).Include(c => c.category).ToList());
        }

        public ActionResult SearchCategory(string searching)
        {
            return View(db.Products.Where(n => n.category.name_category.Contains(searching) || searching == null).Include(c => c.category).ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = db.Products.Include(c => c.category).SingleOrDefault(t => t.id_product == id);

            if (product != null)
            {
                ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");
                return View(product);
            }
            else return HttpNotFound();

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = db.Products.Find(id);

            if (product != null)
            {
                ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");
                return View(product);
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            db.Entry(product).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.Include(c => c.category).SingleOrDefaultAsync(t => t.id_product == id);

            ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}