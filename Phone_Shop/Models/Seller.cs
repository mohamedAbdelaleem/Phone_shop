using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class Seller
    {
        [Required]
        [Key, ForeignKey("IdentityUser")]
        public string user_id { get; set; }

        [Required]
        [MaxLength(14)]
        [MinLength(14)]
        public string national_Id { get; set; }

        public IdentityUser IdentityUser { get; set; }
    }
}
