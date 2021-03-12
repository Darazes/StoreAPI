using StoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Product product = db.Products.Find(id);

            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}