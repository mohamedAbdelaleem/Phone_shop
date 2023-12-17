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
        [Required(ErrorMessage = "Governorate is required")]
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        [Required(ErrorMessage = "City is required")]   
        [ForeignKey("City")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Address Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a valid phone number.")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        public string PhoneNumber { get; set; }
        public IdentityUser User { get; set; }
        public virtual Governorate Governorate { get; set; }
        public virtual City City { get; set; }
    }
}