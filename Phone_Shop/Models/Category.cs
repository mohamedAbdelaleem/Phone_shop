using System.ComponentModel.DataAnnotations;

namespace Phone_Shop.Models
{
    public class Category
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string name { get; set; }
    }
}
