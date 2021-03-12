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
        public int id_delivery{ get; set; }
        [Required]
        public float cost_delivery { get; set; }
        [Required]
        public DateTime date_delivery { get; set; }
        [Required]
        public bool delivered { get; set; }
        [Required]
        public int id_type_delivery { get; set; }
        public Type type { get; set; }

        public ICollection<Shipment> shipments { get; set; }

        public Delivery()
        {
            shipments = new List<Shipment>();
        }


    }
}