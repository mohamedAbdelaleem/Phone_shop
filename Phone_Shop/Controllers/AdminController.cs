using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public IActionResult Home()
        {

            var inActiveProducts = dbContext.Product.Where(p => !p.IsActive).OrderByDescending(p => p.CreatedAt).ToList();

            ViewData["inActiveProducts"] = inActiveProducts;
            
            return View();
        }

        [HttpGet]
        public IActionResult ProductConfirmation(int id)
        {

            var product = dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }
            

            var account = dbContext.Account.SingleOrDefault(a => a.Id == product.SellerId);

            ViewData["product"] = product;
            ViewData["account"] = account;

            return View();
        }

        [HttpPost]
        [ActionName("ProductConfirmation")]
        public IActionResult Confirm(int id) {
            var product = dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }

            product.IsActive = true;
            dbContext.SaveChanges();

            return RedirectToAction("Home", "Admin");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }

            
            dbContext.Product.Remove(product);
            dbContext.SaveChanges();

            return RedirectToAction("Home", "Admin");
        }

    }
}
