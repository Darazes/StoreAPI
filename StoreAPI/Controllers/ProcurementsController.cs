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
    public class ProcurementsController : Controller
    {
        private StoreContext db = new StoreContext();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.Procurements.Include(x =>x.product).ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Procurement procurement = await db.Procurements.Include(p => p.product).FirstOrDefaultAsync(tp => tp.id_procurement == id);

            if (procurement == null)
            {
                return HttpNotFound();
            }
            return View(procurement);
        }

        [Authorize]
        public ActionResult Create()
        {

            ViewBag.id_product= new SelectList(db.Products, "id_product", "name_product");


            return View();
        }

        // POST: Procurements/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id_procurement,date_procurement,cost_procurement,count_procurement,id_storage,id_product")] Procurement procurement)
        {

            if (ModelState.IsValid)
            {
                db.Procurements.Add(procurement);

                if (db.Product_storage.Find(procurement.id_product) == null)
                {
                    db.Product_storage.Add(new Product_storage(procurement.count_procurement, procurement.id_product));
                }
                else 
                {
                    db.Product_storage.Find(procurement.id_product).id_product = procurement.id_product;
                    db.Product_storage.Find(procurement.id_product).count += procurement.count_procurement;
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(procurement);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.id_product = new SelectList(db.Products, "id_product", "name_product");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procurement procurement = await db.Procurements.FindAsync(id);
            if (procurement == null)
            {
                return HttpNotFound();
            }
            return View(procurement);
        }

        // POST: Procurements/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id_procurement,date_procurement,cost_procurement,count_procurement,id_storage,id_product")] Procurement procurement)
        {

            if (ModelState.IsValid)
            {
                db.Entry(procurement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(procurement);
        }

        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procurement procurement = await db.Procurements.Include(x => x.product).SingleOrDefaultAsync(i => i.id_procurement == id);
            if (procurement == null)
            {
                return HttpNotFound();
            }
            return View(procurement);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Procurement procurement = await db.Procurements.Include(x =>x.product).SingleOrDefaultAsync(i => i.id_procurement == id);
            db.Procurements.Remove(procurement);



            if (db.Product_storage.Find(procurement.id_product) != null)
            { 
                db.Product_storage.Find(procurement.id_product).count -= procurement.count_procurement;
            }


            await db.SaveChangesAsync();
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
