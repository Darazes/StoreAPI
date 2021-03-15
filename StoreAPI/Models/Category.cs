using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Category
    {
        [Key]
        public int id_category { get; set; }
        [Required, MaxLength(20)]
        [Display(Name = "Название категории")]
        public string name_category { get; set; }

        public ICollection<Product> products { get; set; }

        public Category()
        {
            products = new List<Product>();
        }   

    }
}