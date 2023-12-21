using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string? search, int PageNumber=1,int LowestPrice = -1,int MaxmiumPrice = int.MaxValue,bool Des=false)
        {
            var Result = _context.Product.Where(product => (product.IsActive && product.Price >= LowestPrice && product.Price <= MaxmiumPrice && product.Amount > 0));
            if (Des)
                Result = Result.OrderByDescending(p => p.Price);
            else
                Result = Result.OrderBy(p => p.Price);
            if (search != null)
            {
                Result = SearchProducts(search);
            }
            List<Product> result = new List<Product>();
            ViewData["LastPageNumber"] = (int)Math.Ceiling(Result.Count() / 9.0);
            ViewData["pagenumber"] = PageNumber;
            ViewData["MaxmiumPrice"] = MaxmiumPrice;
            ViewData["LowestPrice"] = LowestPrice;
            ViewData["Des"] = Des;
            int ca = 0;
            foreach (var product in Result)
            {
                ca++;
                if (ca >= 9 * (PageNumber - 1) + 1 && ca <= 9 * PageNumber )
                    result.Add(product);
            }
            if (Des)
                return View(result.OrderByDescending(p=>p.Price));
              
            else
                return View(result.OrderBy(p => p.Price));

         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IQueryable<Product> SearchProducts(string searchInput)
        {
            var products = _context.Product.Where(p => EF.Functions.FreeText(p.Name, searchInput)
                                                        || EF.Functions.FreeText(p.Description, searchInput));
            
            return products;

        }

    }
}