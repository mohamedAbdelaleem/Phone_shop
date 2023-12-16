using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Phone_Shop.ViewModel;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Grpc.Core;
namespace Phone_Shop.Controllers
{

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public const string CartSessionKey = "CartId";
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        public ActionResult Checkout()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _db);
            Order order = new Order();
            cart.CreateOrder(order);

            return RedirectToAction("OrderConfirmation");
        }
        public ActionResult AddToCart(int Id)
        {
            var addedProduct = _db.Product
                .Single(product => product.Id == Id);

            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            cart.AddToCart(addedProduct);

            return RedirectToAction("ProductDetail", "Product", new {id=Id});
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int ProductId)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _db);
            var removedItem = _db.ShoppingCartItems
                .SingleOrDefault(item => item.ProductId == ProductId && item.CartId==cart.ShoppingCartId);
            if (removedItem != null)
            {
                _db.ShoppingCartItems.Remove(removedItem);
                _db.SaveChanges();
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
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
