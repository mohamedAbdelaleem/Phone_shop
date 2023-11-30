using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class OrderItem
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public string ProductID { get; set; }
        public Product Product { get; set; }

        [Required]
        public int UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }


    }
}
