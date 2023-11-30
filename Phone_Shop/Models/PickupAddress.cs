using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class PickupAddress
    {
        [Required]
        [Key, ForeignKey("Address")]
        public string address_id { get; set; }
        [Required]
        [ForeignKey("AspNetUser")]
        public string user_id { get; set; }

        public Address Address { get; set; }
        public IdentityUser AspNetUser { get; set; }
    }
}