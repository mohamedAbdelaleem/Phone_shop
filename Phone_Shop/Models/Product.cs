using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Phone_Shop.Data.Migrations;

namespace Phone_Shop.Models
{
    public class Product
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        [ForeignKey("Seller")]
        public string seller_id { get; set; }

        [Required]
        [ForeignKey("Category")]
        public string category_id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public string imgUrl { get; set; }

        [Required]
        public int price { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        public Seller Seller { get; set; }

        public Category Category { get; set; }

        public ProductAddress ProductAddress { get; set; }

    }
}
