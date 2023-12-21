using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;

namespace Phone_Shop.Controllers
{
    public class ReviewController : Controller
    {

        private readonly ApplicationDbContext dbContext;

        public ReviewController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }


}
