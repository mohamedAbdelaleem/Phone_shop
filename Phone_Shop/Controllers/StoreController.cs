
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public StoreController(ApplicationDbContext context, UserManager<IdentityUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }
        public IActionResult Index()
        {
            var sellerid = _userManager.GetUserId(User);
            var result = _context.Store.Where(p => p.SellerId == sellerid).ToList();
            return View(result);
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Create()
        {
            var governoratesInEgypt = new List<string>
        {
        "Cairo",
        "Alexandria",
        "Giza",
      "Dakahlia",
       "Red Sea",
        "Beheira",
        "Fayoum",
       "Gharbiya",
       "Ismailia",
        "Menofia",
         "Minya",
        "Qaliubiya",
       "New Valley",
        "Suez",
        "Aswan",
        "Assiut",
        "Beni Suef",
       "Port Said",
     "Damietta",
     "Sharkia",
     "South Sinai",
      "Kafr Al sheikh",
      "Matrouh",
       "Luxor",
       "Qena",
      "North Sinai",
        "Sohag"

        };

            ViewBag.GovernoratesInEgypt = governoratesInEgypt;
            return View();
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Store model)
        {
            var sellerId = _userManager.GetUserId(User);
            model.SellerId = sellerId;
            try
            {
                _context.Store.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                var governoratesInEgypt = _context.Governorates.ToList();
                return View();
            }
            
          

           
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Delete(int? id)
        {
            var result = _context.Store.Find(id);

            if (result != null)
            {
               var associatedProducts = _context.Product.Where(p => p.StoreId == id).ToList();

                if (associatedProducts.Count > 0)
                {
                    ViewBag.ErrorMessage = "You must delete the associated products before deleting the store.";
                    return View();
                }
                

                    _context.Store.Remove(result);
                    _context.SaveChanges();
                
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
