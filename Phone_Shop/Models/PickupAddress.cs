using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Phone_Shop.Models
{
    public class PickupAddress
    {
        [Key]
        public string AddressId { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
        public Order Order { get; set; }
    }
}