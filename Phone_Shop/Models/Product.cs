using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Phone_Shop.Data.Migrations;

namespace Phone_Shop.Models
{
    public class Product
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public string SellerId { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        [ForeignKey("Store")]
        public int StoreId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string ImgUrl { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = false;

        public Category Category { get; set; }

        public Store Store { get; set; }

        public IdentityUser Seller { get; set; }

    }
}