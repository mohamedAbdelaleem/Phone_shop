using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using Phone_Shop.Services;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly INotification _notificationService;
        public AdminController(ApplicationDbContext dbContext, INotification notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;

        }

        public IActionResult Home()
        {

            var inActiveProducts = _dbContext.Product.Where(p => !p.IsActive).OrderByDescending(p => p.CreatedAt).ToList();

            ViewData["inActiveProducts"] = inActiveProducts;
            
            return View();
        }

        [HttpGet]
        public IActionResult ProductConfirmation(int id)
        {

            var product = _dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }
            

            var account = _dbContext.Account.SingleOrDefault(a => a.Id == product.SellerId);

            ViewData["product"] = product;
            ViewData["account"] = account;

            return View();
        }

        [HttpPost]
        [ActionName("ProductConfirmation")]
        public IActionResult Confirm(int id) {
            var product = _dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }

            product.IsActive = true;
            _dbContext.SaveChanges();
            _notificationService.SendProductAccepted(product.SellerId, product.Name);

            return RedirectToAction("Home", "Admin");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _dbContext.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || product.IsActive)
            {
                return RedirectToAction("Home", "Admin");
            }

            var productName = product.Name;

            _dbContext.Product.Remove(product);
            _dbContext.SaveChanges();
            _notificationService.SendProductRejected(product.SellerId, productName);
            return RedirectToAction("Home", "Admin");
        }

    }
}
