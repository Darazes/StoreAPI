using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using StoreAPI.Models;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
    public class RequestsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Requests
        public async Task<ActionResult> Index()
        {
            ViewBag.account = User.Identity.Name; 

            var requests = db.Requests.Include(c => c.customer).Include(d => d.delivery);

            return View(await requests.ToListAsync());
        }

        [HttpPost]
        [Route("api/[controller]")]
        public string CreateJson(RequestCustom model)
        {

            if (db.Customers.Where(u => u.id_customer == model.id_customer).FirstOrDefault() != null)
            {
                Request request = new Request();

                List<Request> requests_list = db.Requests.ToList();
                List<Delivery> deliveries_list = db.Deliveries.ToList();

                if (requests_list.Where(r => r.id_request == 1).FirstOrDefault() == null) request.id_request = 1;
                else request.id_request = db.Requests.ToList().LastOrDefault().id_customer + 1;

                //Delivery(пустой для заполнения требуется подтверждение заказа)
                Delivery delivery = new Delivery();
                delivery.id_delivery = request.id_request;
                delivery.date_delivery = DateTime.Now;
                delivery.date_confirm = DateTime.Now;
                delivery.cost_delivery = 0;
                delivery.delivered = false;
                delivery.id_type_delivery = model.id_type_delivery;

                delivery.type = null;
                db.Deliveries.Add(delivery);

                request.date_request = DateTime.Now;
                request.id_customer = model.id_customer;
                request.id_delivery = request.id_request;
                request.status = 1;


                if (db.Requests.Where(u => u.id_request == request.id_request).FirstOrDefault() == null)
                {
                    db.Requests.Add(request);
                    db.SaveChanges();
                }
                else
                {
                    return "Ошибка создания заказа, такой заказ уже существует, попробуйте ещё раз";
                }

                Request request_db = db.Requests.Where(u => u.id_request == request.id_request).FirstOrDefault();

                if (request_db != null) return request.id_request.ToString();

                else return "Ошибка добавления пользователя";

            }

            return "Вы не авторизованы / Такого пользователя не существует";
        }

        [HttpPost]
        [Route("api/[controller]")]
        public string AddProductJson(Product_request_custom model)
        {

            var product_Request_list = db.Product_requests.ToList();
            Product_request product_Request = new Product_request();

            product_Request.id_request = model.id_request;

            if (product_Request_list.Where(p => p.id_product_request == 1).FirstOrDefault() == null) product_Request.id_product_request = 1;
            else product_Request.id_product_request = product_Request_list.LastOrDefault().id_product_request + 1;

            product_Request.count = model.count;

            product_Request.id_product = model.id_product;

            if (db.Product_requests.Where(u => u.id_product_request == product_Request.id_product_request).FirstOrDefault() == null)
            {
                var product_count = db.Product_requests.Where(p => p.id_product == product_Request.id_product).FirstOrDefault();

                if (product_count != null)
                {
                    product_Request.count += product_count.count;
                    db.Product_requests.Remove(product_count);
                }

                db.Product_requests.Add(product_Request);

                if (db.Products.Where(p => p.id_product == product_Request.id_product).FirstOrDefault() != null)
                {
                    db.SaveChanges();
                    return "Товар успешно добавлен в заказ";
                }
                return "Такого товара не существует";
            }
            else return "Товар c таким id уже есть";

        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.delivery).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [HttpGet]
        public async Task<ActionResult> СonfirmDetails(int? id)
        {
            ViewBag.dateShipment = DateTime.Now;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.delivery).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            if (request.product_requests.Count() == 0) ViewBag.error = "В заказе нет товаров";

            return View(request);
        }


        [HttpPost]
        public async Task<ActionResult> Сonfirm(int? id)
        {

            var request = await db.Requests.Include(r => r.product_requests).Include(r => r.delivery).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);

            var request_db = await db.Requests.FindAsync(id);
            var delivery = await db.Deliveries.FindAsync(request.delivery.id_delivery);

            //Подтверждение заказа
            if (request.product_requests.Count() != 0)
            {
                delivery.date_confirm = DateTime.Now;

                request_db.status = 2;

                db.Entry(request_db).State = EntityState.Modified;
                db.Entry(delivery).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Details" + "/" + id, "Requests");
            }
            else
            {
                return RedirectToAction("СonfirmDetails" + "/" + id, "Requests");
            }

            
        }

        // GET: Requests/Create
        //public ActionResult Create()
        //{
        //    ViewBag.id_customer = new SelectList(db.Customers, "id_customer", "login");
        //    ViewBag.id_delivery = new SelectList(db.Deliveries, "id_delivery", "id_delivery");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "id_request,date_request,id_customer,id_delivery,confirm")] Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Requests.Add(request);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.id_customer = new SelectList(db.Customers, "id_customer", "login", request.id_customer);
        //    ViewBag.id_delivery = new SelectList(db.Deliveries, "id_delivery", "id_delivery", request.id_delivery);
        //    return View(request);
        //}

        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = await db.Requests.FindAsync(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.id_customer = new SelectList(db.Customers, "id_customer", "login", request.id_customer);
        //    ViewBag.id_delivery = new SelectList(db.Deliveries, "id_delivery", "id_delivery", request.id_delivery);
        //    return View(request);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "id_request,date_request,id_customer,id_delivery,confirm")] Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(request).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.id_customer = new SelectList(db.Customers, "id_customer", "login", request.id_customer);
        //    ViewBag.id_delivery = new SelectList(db.Deliveries, "id_delivery", "id_delivery", request.id_delivery);
        //    return View(request);
        //}

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = await db.Requests.FindAsync(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(request);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Request request = await db.Requests.FindAsync(id);
        //    db.Requests.Remove(request);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
