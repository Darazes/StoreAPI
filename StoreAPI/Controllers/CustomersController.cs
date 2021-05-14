using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using StoreAPI.Models;
using System.Web.Security;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace StoreAPI.Controllers
{
    public class CustomersController : Controller
    {
        private StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public string IndexJson()
        {

            IEnumerable<Customer> customers = db.Customers.ToList();

            string json = JsonConvert.SerializeObject(customers);

            return json;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string login, string password)
        {
            if (login == "" || password == "")
            {
                ViewBag.error = "Заполните все поля";
                return View();
            }
            // поиск пользователя в бд
            Customer customer = null;

            customer = db.Customers.FirstOrDefault(u => u.login == login && u.password == password);

            
            if (customer != null)
            {
                FormsAuthentication.SetAuthCookie(login, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "Такого пользователя не существует";
                return View();
            }
            
        }

        [HttpPost]
        [Route("api/[controller]")]
        public string LoginJson(Account model)
        { 
            if (model.login == "" || model.password == "")
            {
                return "Заполните все поля";
            }
            // поиск пользователя в бд
            Customer customer = null;

            customer = db.Customers.FirstOrDefault(u => u.login == model.login && u.password == model.password);


            if (customer != null)
            {
                FormsAuthentication.SetAuthCookie(model.login, true);
                return "Успешный вход";
            }
            else
            {
                return "Такого пользователя не существует";
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "id_customer,login,password,phone,adress_customer")] Customer model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = null;

                customer = db.Customers.FirstOrDefault(u => u.login == model.login);

                if (customer == null)
                {
                   
                    model.roleid = 2;

                    db.Customers.Add(model);
                    db.SaveChanges();

                    customer = db.Customers.Where(u => u.login == model.login && u.password == model.password).FirstOrDefault();
                    
                    // если пользователь удачно добавлен в бд
                    if (customer != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Такого пользователя не существует";
                    return View();
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("api/[controller]")]
        public string RegisterJson(CustomerCustom model)
        {
            Customer customer = new Customer
            {
                id_customer = db.Customers.ToList().LastOrDefault().id_customer + 1,
                login = model.login,
                password = model.password,
                phone = model.phone,
                adress_customer = model.adress_customer,
                roleid = 2,
                role = null,
                requests = new List<Request>()
            };


            if (db.Customers.Where(u => u.login == customer.login).FirstOrDefault() == null)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
            else
            {
                return "Пользователь " + customer.login + " уже существует";
            }
            Customer customer_db = db.Customers.Where(u => u.login == customer.login && u.password == customer.password).FirstOrDefault();

            if (customer_db != null) return "Пользователь " + customer.login + " успешно добавлен";
            else return "Ошибка добавления пользователя";

        }

        public ActionResult Logoff()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ViewBag.account = "Войти";
                return RedirectToAction("Login", "Customers");
            }
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.account = User.Identity.Name;

            return View(await db.Customers.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Account()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.Where(c => c.login == User.Identity.Name).FirstOrDefaultAsync();
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id_customer,login,password,phone,adress_customer")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            db.Customers.Remove(customer);
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
