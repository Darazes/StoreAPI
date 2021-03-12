using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    [Table("Product_shipments")]
    public class Product_shipment
    {
        [Key]
        public int id_product_shipment { get; set; }

        [Required]
        public int id_shipment { get; set; }

        public Shipment shipment { get; set; }
        
        [Required]
        public int id_product { get; set; }

        public Product product { get; set; }

        [Required]
        public int count { get; set; }
    }
}