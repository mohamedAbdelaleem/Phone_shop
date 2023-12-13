using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Phone_Shop.Data;
using Phone_Shop.ViewModel;
using System.Web.Helpers;

namespace Phone_Shop.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _db;
        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";
        public ShoppingCart(ApplicationDbContext db)
        {
            _db = db;
        }
        public static ShoppingCart GetCart(HttpContext context, ApplicationDbContext db)
        {
            var cart = new ShoppingCart(db);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller,ApplicationDbContext db)
        {
            return GetCart(controller.HttpContext,db);
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey ,context.User.Identity.Name);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }
        public int RemoveFromCart(int id)
        {

            // Get the cart
            var cartItem = _db.ShoppingCartItems.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    _db.ShoppingCartItems.Remove(cartItem);
                }
                // Save changes
                _db.SaveChanges();
            }
            return itemCount;

        }
        public void AddToCart(Product product)
        {
            // Get the matching cart and product instances
            var cartItem = _db.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                _db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Quantity++;
            }
            // Save changes
            _db.SaveChanges();
        }
        public void EmptyCart()
        {
            var cartItems = _db.ShoppingCartItems.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _db.ShoppingCartItems.Remove(cartItem);
            }
            // Save changes
            _db.SaveChanges();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _db.ShoppingCartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total

            decimal? total = (from cartItems in _db.ShoppingCartItems
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Quantity *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductID = item.ProductId,
                    OrderID = order.Id,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Quantity * item.Product.Price);

                _db.OrderItem.Add(orderItem);

            }
            // Set the order's total to the orderTotal count
            order.TotalPrice = orderTotal;

            // Save the order
            _db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.Id;
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _db.ShoppingCartItems.Where(
                c => c.CartId == ShoppingCartId);

            foreach (CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }
            _db.SaveChanges();
        }
        public List<CartItem> GetCartItems()
        {
            return _db.ShoppingCartItems.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }
    }
}
