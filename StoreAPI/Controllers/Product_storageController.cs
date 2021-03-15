using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    public class Product_storageController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Product_storage
        public async Task<ActionResult> Index()
        {

            ViewBag.id_product = new SelectList(db.Products, "id_product", "name_product");

            var allstorages = await db.Product_storage.Include(p => p.product).ToListAsync();

            return View(allstorages);
        }

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
