using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Phone_Shop.Models
{
    public class ProductAddress
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        [ForeignKey("Product")]
        public string ProductId { get; set; }

        [Required]
        public string Governace { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }

        public Product Product { get; set; }
    }
}
