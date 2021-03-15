using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Procurement
    {
        [Key]

        public int id_procurement { get; set; }
        [Required]
        [Display(Name = "Дата поставки товаров")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date_procurement { get; set; }

        [Required]
        [Display(Name = "Количество товаров")]
        public int count_procurement { get; set; }

        [Required]
        [Display(Name = "Название товара")]
        public int id_product { get; set; }

        public Product product { get; set; }

    }
}