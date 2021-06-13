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
        [Required, MaxLength(20)]
        public string login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string password { get; set; }

        [Display(Name = "Телефон")]
        [Required, MaxLength(11)]
        public string phone { get; set; }

        [Display(Name = "Адрес")]
        [Required, MaxLength(100)]
        public string adress_customer { get; set; }

        public int roleid { get; set; }

        public Role role { get; set; }

        public ICollection<Request> requests { get; set; }
        public ICollection<Favorite> favorites { get; set; }
        public ICollection<Feedback> feedbacks { get; set; }

        public Customer()
        {
            requests = new List<Request>();
            favorites = new List<Favorite>();
            feedbacks = new List<Feedback>();
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

    public class AccountStatus
    {
        public string status  { get; set; }
    }

    public class Profile
    {
        public int id_customer { get; set; }
        public string login { get; set; }
        public string phone { get; set; }
        public string adress_customer { get; set; }

    }

    public class ID
    {
        public int id_customer { get; set; }
    }
}