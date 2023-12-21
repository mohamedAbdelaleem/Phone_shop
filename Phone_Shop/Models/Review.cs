using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Phone_Shop.Models
{
    public class Review
    {

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
        public IdentityUser Customer { get; set; }

    }
}
