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
        [Display(Name = "Номер заказа")]
        public int id_request { get; set; }
        
        [Required]
        [Display(Name = "Дата заказа")]
        public DateTime date_request { get; set; }
       
        [Required]
        [Display(Name = "Заказчик")]
        public int id_customer { get; set; }

        public Customer customer { get; set; }
       
        [Required]
        [Display(Name = "Номер доставки")]
        public int id_delivery { get; set; }

        public Delivery delivery { get; set; }

        [Required]
        [Display(Name = "Статус заказа")]
        public int status { get; set; }

        public ICollection<Product_request> product_requests { get; set; }

        public Request()
        {
            product_requests = new List<Product_request>();
        }
    }

    public class RequestCustom
    {
        public int id_customer { get; set; }

        public int id_type_delivery { get; set; }
    }
}