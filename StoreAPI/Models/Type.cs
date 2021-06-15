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
        public string name_type_delivery { get; set; }

        [Required]
        [Display(Name = "Цена доставки")]
        public float cost_type_delivery { get; set; }

        public ICollection<Request> requests { get; set; }

        public Type()
        {
            requests = new List<Request>();
        }


    }

    public class TypeCustom
    {

        public int id_type_delivery { get; set; }

        public string name_type_delivery { get; set; }

        public float cost_type_delivery { get; set; }

    }
}