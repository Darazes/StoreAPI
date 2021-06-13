using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    public class FavoritesController : Controller
    {

        private StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public string IndexJson()
        {

            List<Favorite> favorites = db.Favorites.ToList();

            string json = JsonConvert.SerializeObject(favorites);

            return json;
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string UserFavorites(FavoritesUser model)
        {

            if (db.Favorites.Where(c => c.id_customer == model.id_customer).ToList() != null && model != null && db.Customers.Where(c => c.id_customer == model.id_customer).FirstOrDefault() != null)
            {
            
                List<Favorite> favorites = db.Favorites.Where(c => c.id_customer == model.id_customer).ToList();

                string json = JsonConvert.SerializeObject(favorites);

                return json;
            }
            return "В избранном нет ни одного товара";
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string AddFavorite(FavoriteCustom model)
        {

            if (db.Favorites.Where(p => p.id_product == model.id_product).Where(c => c.id_customer == model.id_customer).FirstOrDefault() == null &&model != null)
            {

                Favorite favorite = new Favorite();

                List<Favorite> favorites = db.Favorites.ToList();

                if (favorites.Where(r => r.id_favorite == 1).FirstOrDefault() == null) favorite.id_favorite = 1;
                else favorite.id_favorite = favorites.LastOrDefault().id_favorite + 1;
                
                if (db.Products.Where(p=>p.id_product == model.id_product).ToList().FirstOrDefault() == null) return "Нет такого продукта";
                else favorite.id_product = model.id_product;
                if (db.Customers.Where(c => c.id_customer == model.id_customer).ToList().FirstOrDefault() == null) return "Нет такого пользователя";
                else favorite.id_customer = model.id_customer;

                db.Favorites.Add(favorite);
                db.SaveChanges();

                string json = JsonConvert.SerializeObject(favorite);

                return json;
            }

            Favorite favoriteBase = db.Favorites.Where(p => p.id_product == model.id_product).Where(c => c.id_customer == model.id_customer).FirstOrDefault();
            string jsonBase = JsonConvert.SerializeObject(favoriteBase);
            return jsonBase;

        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string GetFavorite(FavoriteCustom model)
        {
            Favorite favoriteBase = db.Favorites.Where(p => p.id_product == model.id_product).Where(c => c.id_customer == model.id_customer).FirstOrDefault();
            if (favoriteBase != null)
            {
                string jsonBase = JsonConvert.SerializeObject(favoriteBase);
                return jsonBase;
            }
            else return "Не в избранном";
        }


        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string DeleteFavorite(FeedBackID model)
        {
            if (model != null)
            {
                Favorite favorite = db.Favorites.Where(c => c.id_customer == model.id_customer).ToList().FirstOrDefault();

                Favorite favoriteDelete = db.Favorites.Find(favorite.id_favorite);

                db.Favorites.Remove(favoriteDelete);
                db.SaveChangesAsync();
                return "Успешно";
            }
            else return "Провал";
        }

        }
}