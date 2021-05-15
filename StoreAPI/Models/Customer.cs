using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Customer
    {
        [Key]
        public int id_customer { get; set; }

        [Display(Name = "Логин")]
        [Required]
        public string login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string password { get; set; }

        [Display(Name = "Телефон")]
        [Required]
        public string phone { get; set; }

        [Display(Name = "Адрес")]
        [Required]
        public string adress_customer { get; set; }

        public int roleid { get; set; }

        public Role role { get; set; }

        public ICollection<Request> requests { get; set; }

        public Customer()
        {
            requests = new List<Request>();
        }

    }

    public class CustomerCustom
    {
        public string login { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string adress_customer { get; set; }

    }

    public class Account
    {
        public string login { get; set; }
        public string password { get; set; }

    }
}