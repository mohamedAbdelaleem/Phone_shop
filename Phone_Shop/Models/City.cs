using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Phone_Shop.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Governorate")]
        public int governorate_id { get; set; }
        public  string city_name_ar { get; set; }
        public string city_name_en { get; set; }
        public Governorate Governorate { get; set; }
    }
}
