using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using Phone_Shop.Models;
using System.Diagnostics;

namespace Phone_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int PageNumber=1)
        {
            var Result = _context.Product;
            List<Product>result=new List<Product>();
            ViewData["pagenumber"] = PageNumber;
            int ca = 0;
            foreach (var product in Result)
            {
                if (product.IsActive)
                {
                    ca++;
                    if (ca >= 9 * (PageNumber - 1) + 1 && ca <= 9 * PageNumber)
                        result.Add(product);
                }
            }
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}