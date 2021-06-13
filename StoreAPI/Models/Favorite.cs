using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Favorite
    {
        [Key]
        public int id_favorite { get; set; }

        [Required]
        public int id_product { get; set; }

        [Required]
        public int id_customer { get; set; }
    }

    public class FavoriteCustom
    {
        public int id_product { get; set; }

        public int id_customer { get; set; }
    }

    public class FavoritesUser
    {
        public int id_customer { get; set; }
    }

}