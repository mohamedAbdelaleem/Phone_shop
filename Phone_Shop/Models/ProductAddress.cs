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

        public Product Product { get; set; }
    }
}
