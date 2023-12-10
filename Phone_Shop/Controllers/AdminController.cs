using Microsoft.AspNetCore.Authorization;
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

            var inActiveProducts = dbContext.Product.Where(x => !x.IsActive).ToList();

            ViewData["inActiveProducts"] = inActiveProducts;


            return View();
        }
    }
}
