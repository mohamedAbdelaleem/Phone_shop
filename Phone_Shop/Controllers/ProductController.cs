using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductController(ApplicationDbContext context, UserManager<IdentityUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }
        public IActionResult Index()
        {
            var sellerid = _userManager.GetUserId(User);
            var result = _context.Product.Include(x => x.Category).Include(x => x.Store).Where(p => p.SellerId == sellerid).ToList();
            return View(result);
        }
        public IActionResult Create()
        {
            ViewBag.Category = _context.Category.OrderBy(x => x.Name).ToList();
            ViewBag.Store = _context.Store.OrderBy(x => x.Name).ToList();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            var sellerId = _userManager.GetUserId(User);
            model.SellerId = sellerId;
            model.CreatedAt= DateTime.Now;
            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {

                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine("wwwroot", "imj", imageName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file[0].CopyTo(fileStream); // Save in the Images folder
                }

                model.ImgUrl = $"/imj/{imageName}"; // Save in the database
            }
            else
            {
                model.ImgUrl = "/imj/defaultImage.jpg";
            }

            _context.Product.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Category = _context.Category.OrderBy(x => x.Name).ToList();
            ViewBag.Store = _context.Store.OrderBy(x => x.Name).ToList();
            var result = _context.Product.Find(id);
            return View("Create", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine("wwwroot", "Images", imageName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file[0].CopyTo(fileStream); // Save in the Images folder
                }

                model.ImgUrl = imageName; // Save in the database
            }
            else
            {
                model.ImgUrl = model.ImgUrl;
            }
            var sellerId = _userManager.GetUserId(User);
            model.SellerId = sellerId;
            _context.Product.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int? id)
        {
            var result = _context.Product.Find(id);
            if (result != null)
            {
                _context.Product.Remove(result);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
