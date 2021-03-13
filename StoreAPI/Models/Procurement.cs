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
        public DateTime date_procurement { get; set; }
        [Required]
        public float cost_procurement { get; set; }
        [Required]
        public int count_procurement { get; set; }

        [Required]
        public int id_product { get; set; }

        public Product product { get; set; }

    }
}