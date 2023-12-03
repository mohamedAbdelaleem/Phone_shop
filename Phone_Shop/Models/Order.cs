using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace Phone_Shop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public int PickupAddressId { get; set; }

        [Required]
        public DateTime OrderedAt { get; set; }

        public int TotalPrice { get; set; }
        public bool Status { get; set; }

        public PickupAddress PickupAddress { get; set; }
        public IdentityUser User { get; set; }

    }
}