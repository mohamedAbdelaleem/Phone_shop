using Phone_Shop.Models;

namespace Phone_Shop.ViewModel
{
    public class CheckoutViewModel
    {
        public PickupAddress Address { get; set; }
        public List<Governorate>? Governorates { get; set; }
        public List<City>? Cities { get; set; }

    }
}
