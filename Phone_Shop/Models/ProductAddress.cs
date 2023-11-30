using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class ProductAddress
    {
        [Required]
        [Key, ForeignKey("Address")]
        public string address_id { get; set; }
        [Required]
        [ForeignKey("Product")]
        public string product_id { get; set; }

        public Product Product { get; set; }
        public Address Address { get; set; }
    }
}