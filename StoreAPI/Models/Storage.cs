using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Storage
    {
        [Key]
        public int id_storage { get; set; }
        
        [Required]
        public string adress_storage { get; set; }

        public ICollection<Procurement> procurements { get; set; }

        public ICollection<Product_storage> product_storages { get; set; }

        public Storage()
        {
            procurements = new List<Procurement>();

            product_storages = new List<Product_storage>();
        }



    }
}