using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Request
    {
        [Key]
        public int id_request { get; set; }
        
        [Required]
        public DateTime date_request { get; set; }
       
        [Required]
        public int id_customer { get; set; }

        public Customer customer { get; set; }
       
        [Required]
        public int id_shipment { get; set; }

        public Shipment shipment { get; set; }

        public ICollection<Product_request> product_requests { get; set; }

        Request()
        {
            product_requests = new List<Product_request>();
        }


    }
}