using Newtonsoft.Json;
using StoreAPI.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreAPI.Controllers
{
    public class ProductController : Controller
    {

        StoreContext db = new StoreContext();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            ViewBag.account = User.Identity.Name;

            var products = db.Products.Include(c => c.category);

            return View(products.ToList());
        }


        public string IndexJson()
        {
            List<Product> products = db.Products.ToList();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Search(string searching)
        {
            return View(db.Products.Where(n => n.name_product.Contains(searching) || searching == null).Include(c => c.category).ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult SearchCategory(string searching)
        {
            return View(db.Products.Where(n => n.category.name_category.Contains(searching) || searching == null).Include(c => c.category).ToList());
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    // Получение имени файла
                    //string fileName = System.IO.Path.GetFileName(upload.FileName);

                    // Сохранение файла с новым именем эквивалентным названию товара
                    upload.SaveAs(Server.MapPath("~/Files/" + product.name_product + ".png"));

                    //Изменение имени файла для пути с заменёнными пробелами
                    string namefile = product.name_product.Replace(" ", "%20");

                    // Добавление пути изображения товару в базе

                    product.image_url = ("/Files/" + namefile + ".png");

                    //Добавление товара в базу
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase upload)
        {

            if (upload != null)
            {
                // Получение имени файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);


                //Получение пути изображения
                string fullPath = Server.MapPath("~/Files/" + product.name_product + ".png");

                //Проверка есть ли заданный путь
                if (System.IO.File.Exists(fullPath))
                {
                    //Удаление предыдущего изображения
                    System.IO.File.Delete(fullPath);

                    //Сохранение нового изображения
                    upload.SaveAs(Server.MapPath("~/Files/" + product.name_product + ".png"));

                }

                db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);

            //Получение пути изображения
            string fullPath = Server.MapPath("~/Files/" + product.name_product + ".png");

            //Проверка есть ли заданный путь
            if (System.IO.File.Exists(fullPath))
            {
                //Удаление предыдущего изображения
                System.IO.File.Delete(fullPath);
            }

                await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}