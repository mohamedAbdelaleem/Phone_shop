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
        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            var addedProduct = _db.Product
                .Single(product => product.Id == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            cart.AddToCart(addedProduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext, _db);

            // Get the name of the product to display confirmation
            string productName = _db.ShoppingCartItems
                .Single(item => item.ItemId == id).Product.Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = productName +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [System.Web.Mvc.ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
