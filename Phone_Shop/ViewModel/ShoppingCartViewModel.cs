using Phone_Shop.Models;

namespace Phone_Shop.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}
