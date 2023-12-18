using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Phone_Shop.Models
{
    public class PickupAddress
    {
        [Key]
        public int AddressId { get; set; }
        
        [ForeignKey("User")]
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Governorate is required"),ForeignKey("Governorate"), DisplayName("Governorat")]
        public int GovernorateId { get; set; }
        [Required(ErrorMessage = "City is required"),ForeignKey("City"), DisplayName("City")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Address Description is required"), DisplayName("Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a valid phone number."), DisplayName("Phone Number")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        public string PhoneNumber { get; set; }
        public IdentityUser? User { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public virtual City? City { get; set; }
    }
}