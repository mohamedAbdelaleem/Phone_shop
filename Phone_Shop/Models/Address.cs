using System.ComponentModel.DataAnnotations;

namespace Phone_Shop.Models
{
    public class Address
    {
        [Required]
        public string id { get; set; }
        [Required]

        public string governce { get; set; }
        [Required]

        public string city { get; set; }
        [Required]
        public string street { get; set; }
        public ProductAddress ProductAddress { get; set; }
        public PickupAddress PickupAddress { get; set; }


    }
}