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

        public IActionResult Index(string? search,string? Des, int PageNumber=1,int LowestPrice = -1,int MaxmiumPrice = int.MaxValue)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Home", "Admin");
            }
            if (User.IsInRole("Delivery"))
            {
                return RedirectToAction("Home", "Delivery");
            }
            var Result = _context.Product.Where(product => (product.IsActive && product.Price >= LowestPrice && product.Price <= MaxmiumPrice && product.Amount > 0 && product.Archived == false));
            if (Des!=null && Des.Equals("true"))
                Result = Result.OrderByDescending(p => p.Price);
            else if (Des!=null)
                Result = Result.OrderBy(p => p.Price);
            if (search != null)
            {
                if (Des != null && Des.Equals("true"))
                    Result = SearchProducts(Result, search).OrderByDescending(p=>p.Price);
                else if (Des != null)
                    Result = SearchProducts(Result, search).OrderBy(p => p.Price);

            }
            List<Product> result = new List<Product>();
            ViewData["LastPageNumber"] = (int)Math.Ceiling(Result.Count() / 9.0);
            ViewData["pagenumber"] = PageNumber;
            ViewData["MaxmiumPrice"] = MaxmiumPrice;
            ViewData["LowestPrice"] = LowestPrice;
            ViewData["Des"] = Des;
            ViewData["search"] = search;

            result = Result.AsNoTracking()
               .OrderByDescending(p=>p.CreatedAt)
              .Skip((PageNumber - 1) * 9)
              .Take(9).ToList();

            if (Des != null && Des.Equals("true"))
                return View(result.OrderByDescending(p=>p.Price));
              
            else if (Des != null)
                return View(result.OrderBy(p => p.Price));
            return View(result);
         
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