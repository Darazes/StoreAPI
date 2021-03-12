using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    [Table("Product_requests")]
    public class Product_request
    {
        [Key]
        public int id_product_request { get; set; }

        [Required]
        public int id_request { get; set; }

        Request request { get; set; }

        [Required]
        public int id_product { get; set; }

        Product product { get; set; }

        [Required]
        public int count { get; set; }


        

        
    }
}