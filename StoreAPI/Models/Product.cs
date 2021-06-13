using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Product
    {
        [Key]
        public int id_product { get; set; }

        [Required, MaxLength(40)]
        [Display(Name = "Название товара")]
        public string name_product { get; set; }

        [Required]
        [Display(Name = "Цена товара")]
        public float cost_product { get; set; }

        [Required, MaxLength(400)]
        [Display(Name = "Описание товара")]
        public string content { get; set; }

        [Required]
        public int id_category { get; set; }

        public Category category { get; set; }

        public string image_url { get; set; }

        public ICollection<Procurement> procurements { get; set; }

        public ICollection<Product_request> product_requests { get; set; }

        public ICollection<Product_storage> product_storages { get; set; }

        public ICollection<Favorite> favorites { get; set; }


        public Product()
        {

            procurements = new List<Procurement>();

            product_requests = new List<Product_request>();

            product_storages = new List<Product_storage>();

            favorites = new List<Favorite>();

        }


    }

    public class ProductCustom
    {

        public int id_product { get; set; }

        public string name_product { get; set; }

        public float cost_product { get; set; }

        public string content { get; set; }

        public int id_category { get; set; }

        public string image_url { get; set; }

    }
}