using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    [Table("Product_storages")]
    public class Product_storage
    {
        [Key]
        public int id_product_storage { get; set; }

        [Required]
        public int id_product { get; set; }

        public Product product { get; set; }

        [Required]
        [Display(Name = "Количество товаров на складе")]
        public int count { get; set; }

        public Product_storage()
        { 
        }
        public Product_storage(int count, int id_product)
        {
            this.count = count;
            this.id_product = id_product;
        }

    }
}