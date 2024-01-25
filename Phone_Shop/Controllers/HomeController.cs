using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;
using System.Diagnostics;
using System.Linq;

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
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Home", "Admin");
            }
            if (User.IsInRole("Delivery"))
            {
                return RedirectToAction("Home", "Delivery");
            }
            var Result = _context.Product.Where(product => (product.IsActive && product.Price >= LowestPrice && product.Price <= MaxmiumPrice && product.Amount > 0 && product.Archived == false) );
            if (Des)
                Result = Result.OrderByDescending(p => p.Price);
            else
                Result = Result.OrderBy(p => p.Price);
            if (search != null)
            {
                if (Des)
                    Result = SearchProducts(Result, search).OrderByDescending(p=>p.Price);
                else
                    Result = SearchProducts(Result, search).OrderBy(p => p.Price);

            }
            List<Product> result = new List<Product>();
            ViewData["LastPageNumber"] = (int)Math.Ceiling(Result.Count() / 9.0);
            ViewData["pagenumber"] = PageNumber;
            ViewData["MaxmiumPrice"] = MaxmiumPrice;
            ViewData["LowestPrice"] = LowestPrice;
            ViewData["Des"] = Des;
            ViewData["search"] = search;
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

        public IQueryable<Product> SearchProducts(IQueryable<Product> products, string searchInput)
        {
            // using FreeText function require a full-text index to be configured.
            try
            {
                var result = products.Where(p => EF.Functions.FreeText(p.Name, searchInput)
                                                            || EF.Functions.FreeText(p.Description, searchInput));

                result.Count();

                return result;
            }
            catch (SqlException ex)
            {
                
                var result = products.Where(p => p.Name.Contains(searchInput)
                                                            || p.Description.Contains(searchInput));
                return result;

            }

        }

    }
}