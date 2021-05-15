using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models
{
    public class Role
    {
        public int id { get; set; }

        [Display(Name = "Роль")]
        public string name { get; set; }
    }
}