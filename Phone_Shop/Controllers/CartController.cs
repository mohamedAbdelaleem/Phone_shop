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

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        
        public ActionResult AddToCart(int Id)
        {
            var addedProduct = _db.Product
                .Single(product => product.Id == Id);

            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            cart.AddToCart(addedProduct);

            return RedirectToAction("ProductDetail", "Product", new {id=1});
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _db);

            string productName = _db.ShoppingCartItems
                .Single(item => item.ItemId == id).Product.Name;

            int itemCount = cart.RemoveFromCart(id);

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
        [System.Web.Mvc.ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext,_db);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
