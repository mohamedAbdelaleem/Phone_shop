using Phone_Shop.Models;

namespace Phone_Shop.ViewModel
{
    public class OrderConfirmationViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public Order Order { get; set; }

    }
}
