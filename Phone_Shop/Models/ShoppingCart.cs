using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace Phone_Shop.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _context;
        public static string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";
        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }
        public static ShoppingCart GetCart(HttpContext Httpcontext, ApplicationDbContext context)
        {
            var cart = new ShoppingCart(context);
            if(ShoppingCart.ShoppingCartId==null)
            ShoppingCart.ShoppingCartId = cart.GetCartId(Httpcontext);
            return cart;
        }
        public static ShoppingCart GetCart(Controller controller, ApplicationDbContext context)
        {
            return GetCart(controller.HttpContext, context);
        }
        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }
        public void AddToCart(Product product,int qty)
        {
            var cartItem = _context.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Quantity = qty,
                    DateCreated = DateTime.Now
                };
                _context.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity = Math.Min(product.Amount, cartItem.Quantity + qty);
            }
            _context.SaveChanges();
        }
        public void UpdateCart(Product product, int qty)
        {
            var cartItem = _context.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Quantity = qty,
                    DateCreated = DateTime.Now
                };
                _context.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity = qty;
            }
            _context.SaveChanges();
        }
        public void EmptyCart()
        {
            var cartItems = _context.ShoppingCartItems.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _context.ShoppingCartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }
        public int GetCount()
        {
            int? count = (from cartItems in _context.ShoppingCartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            decimal? total = (from cartItems in _context.ShoppingCartItems
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Quantity *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }
        public void CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductID = item.ProductId,
                    OrderID = order.Id,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity
                };
                orderTotal += (item.Quantity * item.Product.Price);

                _context.OrderItem.Add(orderItem);

            }
            order.TotalPrice = orderTotal;
            _context.SaveChanges();
            EmptyCart();
        }
        public void MigrateCart(string userName)
        {
            var shoppingCart = _context.ShoppingCartItems.Where(
                c => c.CartId == ShoppingCartId);

            foreach (CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }
            
            ShoppingCartId = userName;
            _context.SaveChanges();
        }
        public List<CartItem> GetCartItems()
        {
            List<CartItem> CartItems = _context.ShoppingCartItems.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
            foreach (var item in CartItems)
            {
                item.Product = _context.Product.Single(x => x.Id == item.ProductId);
            }
            return CartItems;
        }
    }
}
