using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace Phone_Shop.Models
{
    public class Store
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [ForeignKey("Seller")]
        public string SellerId { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Governace { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }

        public IdentityUser Seller { get; set; }
    }
}
