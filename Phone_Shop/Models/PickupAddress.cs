using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Phone_Shop.Models
{
    public class PickupAddress
    {
        [Key]
        public int AddressId { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Governace { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }

        public IdentityUser User { get; set; }
    }
}