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

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.account = User.Identity.Name; 

            var requests = db.Requests.Include(c => c.customer);

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

                if (requests_list.Where(r => r.id_request == 1).FirstOrDefault() == null) request.id_request = 1;
                else request.id_request = requests_list.LastOrDefault().id_request + 1;

                //Неподтверждённый заказ без товаров.
                request.date_request = DateTime.Now;
                request.date_confirm = DateTime.Now;
                request.date_delivery = DateTime.Now;
                request.id_customer = model.id_customer;
                request.status = 1;
                request.cost_request = 0;
                request.id_type_delivery = model.id_type_delivery;
                request.type = null;

                if (db.Requests.Where(u => u.id_request == request.id_request).FirstOrDefault() == null)
                {
                    db.Requests.Add(request);
                    db.SaveChanges();
                }
                else
                {
                    return "Ошибка создания заказа, такой заказ уже существует, попробуйте ещё раз" + request.id_request;
                }

                Request request_db = db.Requests.Where(u => u.id_request == request.id_request).FirstOrDefault();

                if (request_db != null) return request.id_request.ToString();

                else return "Ошибка добавления заказа";

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

                db.Product_requests.Add(product_Request);
                db.SaveChanges();

                if (db.Products.Where(p => p.id_product == product_Request.id_product).FirstOrDefault() != null)
                {
                    return "Товар успешно добавлен в заказ";
                }
                return "Такого товара не существует";
            }
            else return "Товар c таким id уже есть";

        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(t=>t.type).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> СonfirmDetails(int? id)
        {
            ViewBag.date = DateTime.Now;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            if (request.product_requests.Count() == 0) ViewBag.error = "В заказе нет товаров";

            foreach (var item in request.product_requests)
            {
                var product = await db.Product_storage.Where(i => i.id_product == item.id_product).Include(p=>p.product).SingleOrDefaultAsync();

                if (product.count < item.count)
                {
                    ViewBag.error = @" Товар "" " + product.product.name_product + @" "" не в наличии.";
                }
            }

            return View(request);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Сonfirm(int? id)
        {

            var request = await db.Requests.Include(r => r.product_requests).SingleOrDefaultAsync(r => r.id_request == id);

            var request_db = await db.Requests.FindAsync(id);

            if (request.product_requests.Count() != 0)
            {

                foreach (var item in request.product_requests)
                {
                    var product = await db.Product_storage.Where(i => i.id_product == item.id_product).SingleOrDefaultAsync();

                    if (product.count < item.count)
                    {
                        return RedirectToAction("СonfirmDetails" + "/" + id, "Requests");
                    } 
                }

                foreach (var item in request.product_requests)
                {
                    var product =  await db.Product_storage.Where(i => i.id_product == item.id_product).SingleOrDefaultAsync();

                    product.count -= item.count;

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }    

                request_db.date_confirm = DateTime.Now;

                request_db.status = 2;

                request_db.cost_request = await CostRequest(id);

                db.Entry(request_db).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Details" + "/" + id, "Requests");
            }
            else
            {
                return RedirectToAction("СonfirmDetails" + "/" + id, "Requests");
            }

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> СonfirmShip(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            return View(request);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> СonfirmShipPost(int? id)
        {

            var request = await db.Requests.FindAsync(id);

            request.status = 3;

            db.Entry(request).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Details" + "/" + id, "Requests");

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> СonfirmDelivery(int? id)
        {
            ViewBag.date = DateTime.Now;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            return View(request);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> СonfirmDeliveryPost(int? id)
        {

            var request = await db.Requests.FindAsync(id);

            request.date_delivery = DateTime.Now;

            request.status = 4;

            db.Entry(request).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Details" + "/" + id, "Requests");

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> CancelDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Include(r => r.product_requests).Include(r => r.customer).SingleOrDefaultAsync(r => r.id_request == id);
            if (request == null)
            {
                return HttpNotFound();
            }

            return View(request);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> СonfirmCancel(int? id)
        {
            var request_db = await db.Requests.Include(r => r.product_requests).SingleOrDefaultAsync(r => r.id_request == id);

            var request = await db.Requests.FindAsync(id);

            if (request.status == 2)
            {
                foreach (var item in request_db.product_requests)
                {
                    var product = await db.Product_storage.Where(i => i.id_product == item.id_product).SingleOrDefaultAsync();

                    product.count += item.count;

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
                request.status = 5;

                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
            

            return RedirectToAction("Details" + "/" + id, "Requests");

        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<string> CancelJson(RequestCancel model)
        {
            var request_db = await db.Requests.Include(r => r.product_requests).SingleOrDefaultAsync(r => r.id_request == model.id_request);

            var request = await db.Requests.FindAsync(model.id_request);

            if (request.status == 3 || request.status == 4 || request.status == 5) return "Отмена невозможна";

            if (request.status == 2)
            {
                foreach (var item in request_db.product_requests)
                {
                    var product = await db.Product_storage.Where(i => i.id_product == item.id_product).SingleOrDefaultAsync();

                    product.count += item.count;

                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

                request.status = 5;

                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return "Заказ отменён";

        }

        [Authorize(Roles = "admin")]
        public async Task<float> CostRequest(int? id)
        {
            var request = await db.Requests.Include(r => r.product_requests).SingleOrDefaultAsync(r => r.id_request == id);

            var type = await db.Types.SingleOrDefaultAsync(i => i.id_type_delivery == request.id_type_delivery);

            float cost = 0;

            foreach (var item in request.product_requests)
            {
                var product = await db.Products.SingleOrDefaultAsync(i => i.id_product == item.id_product);
                cost += (product.cost_product * item.count);

                if (item == request.product_requests.Last())
                {
                    cost += type.cost_type_delivery;
                    return cost;
                }
            }
            return 0;
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
