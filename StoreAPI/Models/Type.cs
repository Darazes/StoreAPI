using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Type
    {
        [Key]
        public int id_type_delivery { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "Тип доставки")]
        public string name_type { get; set; }

        public ICollection<Delivery> deliveries { get; set; }

        public Type()
        {
            deliveries = new List<Delivery>();
        }


    }
}