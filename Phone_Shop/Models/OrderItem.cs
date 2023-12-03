using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderID { get; set; }
        
        [Required]
        public string ProductAddress { get; set; }
        
        [Required]
        public Order Order { get; set; }
        
        [Required]
        public string ProductID { get; set; }
        
        public Product Product { get; set; }

        [Required]
        public int UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }


    }
}