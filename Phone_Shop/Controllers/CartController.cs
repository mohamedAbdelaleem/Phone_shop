using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Phone_Shop.ViewModel;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Grpc.Core;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Authorization;
namespace Phone_Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_context);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _context);
            cart.EmptyCart();
            var jsonData = new
            {
                Message = "Cart cleared successfully."
            };
            return Json(jsonData);
        }
        public ActionResult AddToCart(int Id, int qty)
        {
            var addedProduct = _context.Product
                .Single(product => product.Id == Id);

            if (qty > 0 && qty <= addedProduct.Amount)
            {
                var cart = ShoppingCart.GetCart(this.HttpContext, _context);
                cart.AddToCart(addedProduct,qty);
                TempData["AddToCartMessage"] = "Product added to cart successfully!";
            }
            else
            {
                TempData["AddToCartMessage"] = "Invalid quantity. Please choose a valid quantity.";
            }

            return RedirectToAction("ProductDetail", "Product", new {id=Id});
        }
        public ActionResult UpdateCart(int Id, int qty)
        {
            var addedProduct = _context.Product
                .Single(product => product.Id == Id);

              var cart = ShoppingCart.GetCart(this.HttpContext, _context);
              cart.UpdateCart(addedProduct, qty);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int ProductId)
        {
            var removedItem = _context.ShoppingCartItems
                .SingleOrDefault(item => item.ProductId == ProductId && item.CartId==ShoppingCart.ShoppingCartId);
            if (removedItem != null)
            {
                _context.ShoppingCartItems.Remove(removedItem);
                _context.SaveChanges();
            }

            var jsonData = new
            {
                Message = "Item removed from cart."
            };
            return Json(jsonData);
        }
        [System.Web.Mvc.ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_context);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
