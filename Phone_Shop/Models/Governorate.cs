using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Phone_Shop.Models
{
    public class Governorate
    {
        [Key]
        public int Id { get; set; }
        public string governorate_name_ar { get; set; }
        public string governorate_name_en { get; set; }

    }
}
