using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Shipment
    {
        [Key]
        public int id_shipment { get; set; }

        [Required]
        public DateTime date_shipment { get; set; }

        [Required]
        public int id_delivery { get; set; }

        Delivery delivery { get; set; }

        public ICollection<Request> requests { get; set; }

        public ICollection<Product_shipment> product_shipments { get; set; }

        public Shipment()
        {
            requests = new List<Request>();

            product_shipments = new List<Product_shipment>();
        }

    }
}