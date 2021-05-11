using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Delivery
    {
        [Key]
        [Display(Name = "Номер доставки")]
        public int id_delivery{ get; set; }

        [Required]
        [Display(Name = "Цена доставки")]
        public float cost_delivery { get; set; }

        [Required]
        [Display(Name = "Дата подтверждения заказа")]
        public DateTime date_confirm { get; set; }

        [Required]
        [Display(Name = "Дата доставки")]
        public DateTime date_delivery { get; set; }

        [Required]
        [Display(Name = "Статус доставки")]
        public bool delivered { get; set; }

        [Required]
        [Display(Name = "Тип доставки")]
        public int id_type_delivery { get; set; }
        public Type type { get; set; }

        [Required]
        [Display(Name = "Номер заказа")]


        public ICollection<Request> requests { get; set; }

        public Delivery()
        {
            requests = new List<Request>();
        }


    }
}