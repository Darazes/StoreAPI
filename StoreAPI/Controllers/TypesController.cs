using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using StoreAPI.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StoreAPI.Controllers
{
    public class TypesController : Controller
    {
        private StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<string> IndexJson()
        {

            List<Type> types = await db.Types.ToListAsync();
            List<TypeCustom> typesCustom = new List<TypeCustom>();

            TypeCustom type = new TypeCustom();

            foreach (Type item in types)
            {
                type.id_type_delivery = item.id_type_delivery;
                type.name_type_delivery = item.name_type_delivery;
                type.cost_type_delivery = item.cost_type_delivery;
                typesCustom.Add(type);
            }

            string json = JsonConvert.SerializeObject(typesCustom);

            return json;
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.account = User.Identity.Name;

            return View(await db.Types.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Type type = await db.Types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id_type_delivery,name_type_delivery,cost_type_delivery")] Models.Type  type)
        {
            if (ModelState.IsValid)
            {
                db.Types.Add(type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(type);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Type type = await db.Types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id_type_delivery,name_type_delivery,cost_type_delivery")] Models.Type  type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(type);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Type  type = await db.Types.FindAsync(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Models.Type  type = await db.Types.FindAsync(id);
            db.Types.Remove(type);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
