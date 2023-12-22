using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }
        [Required,ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public CartItem()
        {
            DateCreated = DateTime.Now;
        }
    }
}
