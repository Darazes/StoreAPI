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

        //Открыт для незарегестрированных пользователей
        public string IndexJson()
        {

            List<Product> products = db.Products.ToList();
            List<ProductCustom> newproducts = new List<ProductCustom>();

            foreach (Product item in products)
            {
                ProductCustom product = new ProductCustom();
                product.id_product = item.id_product;
                product.name_product = item.name_product;
                product.cost_product = item.cost_product;
                product.content = item.content;
                product.id_category = item.id_category;
                product.image_url = item.image_url;
                newproducts.Add(product);
            }

            string json = JsonConvert.SerializeObject(newproducts, Formatting.Indented);

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
            ViewBag.no_category = "";
            ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase upload)
        {
            bool image_status = System.IO.File.Exists(Server.MapPath("~/Content/Images/" + product.name_product + ".png"));

            ViewBag.id_category = new SelectList(db.Categories, "id_category", "name_category");

            if (db.Categories.ToList().Count == 0) ViewBag.no_category = "Требуется поле название категории";

            if (image_status == false) ViewBag.no_image = "Требуется изображение товара";

            if (ModelState.IsValid && (db.Categories.ToList().Count != 0) && upload != null)
            {
                if (upload != null)
                {

                    if (!System.IO.File.Exists(Server.MapPath("~/Content/Images")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/Images"));
                    }

                    // Сохранение файла с новым именем эквивалентным названию товара
                    upload.SaveAs(Server.MapPath("~/Content/Images/" + product.name_product + ".png"));

                    //Изменение имени файла для пути с заменёнными пробелами
                    string namefile = product.name_product.Replace(" ", "%20");

                    // Добавление пути изображения товару в базе
                    product.image_url = ("/Content/Images/" + namefile + ".png");

                    //Добавление товара в базу
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(product);

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

            if (ModelState.IsValid && (db.Categories.ToList().Count != 0))
            {
                if (upload != null)
                {
                    //Получение пути изображения
                    string fullPath = Server.MapPath("~/Content/Images/" + product.name_product + ".png");

                    //Проверка есть ли заданный путь
                    if (System.IO.File.Exists(fullPath))
                    {

                        //Удаление предыдущего изображения
                        System.IO.File.Delete(fullPath);

                        //Сохранение нового изображения
                        upload.SaveAs(Server.MapPath("~/Content/Images/" + product.name_product + ".png"));

                        //Изменение имени файла для пути с заменёнными пробелами
                        string namefile = product.name_product.Replace(" ", "%20");

                        // Добавление пути изображения товару в базе
                        product.image_url = ("/Content/Images/" + namefile + ".png");
                    }
                }
                else 
                {
                    string fullPath = Server.MapPath("~/Content/Images/" + product.name_product + ".png");

                    if (System.IO.File.Exists(fullPath))
                    {
                        string namefile = product.name_product.Replace(" ", "%20");

                        product.image_url = ("/Content/Images/" + namefile + ".png");
                    }
                }

                db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
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
            string fullPath = Server.MapPath("~/Images/" + product.name_product + ".png");

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