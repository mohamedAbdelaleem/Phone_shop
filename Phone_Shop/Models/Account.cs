using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Phone_Shop.Models
{
    public class Account
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String? Photo { get; set; }
        public IdentityUser User { get; set; }
    }
}
