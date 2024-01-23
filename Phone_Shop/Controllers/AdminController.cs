using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var store = _dbContext.Store.Single(s => s.Id == product.StoreId);
            var account = _dbContext.Account.SingleOrDefault(a => a.Id == store.SellerId);

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

            var store = _dbContext.Store.Single(s => s.Id == product.StoreId);

            product.IsActive = true;
            _dbContext.SaveChanges();
            _notificationService.SendProductAccepted(store.SellerId, product.Name);

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

            var store = _dbContext.Store.Single(s => s.Id == product.StoreId);
            var productName = product.Name;

            _dbContext.Product.Remove(product);
            _dbContext.SaveChanges();
            _notificationService.SendProductRejected(store.SellerId, productName);
            return RedirectToAction("Home", "Admin");
        }

    }
}
