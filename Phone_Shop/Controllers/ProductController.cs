using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<IdentityUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Index()
        {
            var sellerid = _userManager.GetUserId(User);
            var result = _context.Product.Include(x => x.Category).Include(x => x.Store).Where(p => p.SellerId == sellerid).ToList();
            return View(result);
        }
        [Authorize(Roles = "Seller")]
        public IActionResult Create()
        {
            var sellerid = _userManager.GetUserId(User);
            ViewBag.Category = _context.Category.OrderBy(x => x.Name).ToList();
            ViewBag.Store = _context.Store.Where(p => p.SellerId == sellerid).OrderBy(x => x.Name).ToList();
            return View();
        }



        [Authorize(Roles = "Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            var sellerId = _userManager.GetUserId(User);
            model.SellerId = sellerId;

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
            model.CreatedAt = DateTime.Now;
            Console.WriteLine(DateTime.Now);

            _context.Product.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Seller")]
        public IActionResult Edit(int? id)
        {
            ViewBag.Category = _context.Category.OrderBy(x => x.Name).ToList();
            ViewBag.Store = _context.Store.OrderBy(x => x.Name).ToList();
            var result = _context.Product.Find(id);
            return View("Create", result);
        }


        [Authorize(Roles = "Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
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
                model.ImgUrl = model.ImgUrl;
            }
            var sellerId = _userManager.GetUserId(User);
            model.SellerId = sellerId;
            ;
            model.CreatedAt = DateTime.Now;
            Console.WriteLine(DateTime.Now);
            _context.Product.Update(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Seller")]
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

        public IActionResult ProductDetail(int id)
        {

            var product = _context.Product.SingleOrDefault(p => p.Id == id);
            if (product == null || !product.IsActive)
            {
                return RedirectToAction("Index", "Home");
            }


            var seller = _context.Account.SingleOrDefault(a => a.Id == product.SellerId);

            bool canReviewProduct = false;
            string? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(User);
                canReviewProduct = CanReviewProduct(userId, id);
            }

            var reviews = _context.Review.Where(r => r.ProductID == id).Join(_context.Account,
                                                                             rev => rev.CustomerId,
                                                                             account => account.Id,
                                                                             (rev, account) => new {rev, account});

            var numOfReviews = reviews.Count();
            double averageReviews = 0.0;
            if (numOfReviews >= 1)
            {
               averageReviews = reviews.Average(r => r.rev.Rating);

            }

            bool canReviewProduct = false;
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(User);
                canReviewProduct = CanReviewProduct(userId, id);
            }

            var reviews = _context.Review.Where(r => r.ProductID == id).Join(_context.Account,
                                                                             rev => rev.CustomerId,
                                                                             account => account.Id,
                                                                             (rev, account) => new {rev, account});

            ViewData["product"] = product;
            ViewData["seller"] = seller;
            ViewData["canReviewProduct"] = canReviewProduct;
            ViewData["reviews"] = reviews;
            ViewData["averageReviews"] = averageReviews;
            ViewData["numOfReviews"] = numOfReviews;
            ViewData["userId"] = userId;

            return View();
        }


        // Reviews

        public bool CanReviewProduct(string userId, int productId)
        {

            var review = _context.Review.SingleOrDefault(r => r.CustomerId == userId
                                                               && r.ProductID == productId);

            if (review != null)
            {
                return false;
            }
            var orderItem = _context.Order.Where(o => o.UserId == userId).Join(_context.OrderItem,
                                                                        o => o.Id,
                                                                        oItem => oItem.OrderID,
                                                                        (o, oItem) => oItem)
                                                                        .Where(oItem => oItem.ProductID == productId)
                                                                        .FirstOrDefault();
            
            if (orderItem == null)
            {
                return false;
            }
            var order = _context.Order.SingleOrDefault(o => o.Id == orderItem.OrderID);

            if (order.Status != "delivered")
            {
                return false;
            }

            return true;
        }

        [Authorize(Roles = "Seller,Customer")]
        public IActionResult ReviewProduct(int id)
        {
           
            string userId = _userManager.GetUserId(User);
            bool canReviewProduct = CanReviewProduct(userId, id);
            
            if (!canReviewProduct)
            {
                return RedirectToAction("ProductDetail", "Product", new { id = id });
            }

            ViewData["productId"] = id;
            return View();
        }


        public bool CanReviewProduct(string userId, int productId)
        {

            var review = _context.Review.SingleOrDefault(r => r.CustomerId == userId
                                                               && r.ProductID == productId);

            if (review != null)
            {
                return false;
            }
            var orderItem = _context.Order.Where(o => o.UserId == userId).Join(_context.OrderItem,
                                                                        o => o.Id,
                                                                        oItem => oItem.OrderID,
                                                                        (o, oItem) => oItem)
                                                                        .Where(oItem => oItem.ProductID == productId)
                                                                        .FirstOrDefault();
            
            if (orderItem == null)
            {
                return false;
            }
            var order = _context.Order.SingleOrDefault(o => o.Id == orderItem.OrderID);

            if (order.Status != "delivered")
            {
                return false;
            }

            return true;
        }

        [Authorize(Roles = "Seller,Customer")]
        [HttpPost]
        public IActionResult AddReviewProduct(int id, Review model)
        {

            string userId = _userManager.GetUserId(User);
            bool canReviewProduct = CanReviewProduct(userId, id);

            if (!canReviewProduct)
            {
                return RedirectToAction("ProductDetail", "Product", new { id = id });
            }

            model.ProductID = id;
            model.CustomerId = userId;
            model.CreatedAt = DateTime.Now;

            _context.Review.Add(model);
            _context.SaveChanges();


            return RedirectToAction("ProductDetail", "Product", new { id = id });
        }

        [Authorize(Roles = "Seller,Customer")]
        public IActionResult EditReviewProduct(int productId)
        {

            string userId = _userManager.GetUserId(User);
            var review = _context.Review.SingleOrDefault(r => r.ProductID == productId && r.CustomerId == userId);
            if (review == null) {
                return RedirectToAction("ProductDetail", "Product", new { id = productId });
            }

            

            ViewData["productId"] = productId;
            return View(review);
        }

        [Authorize(Roles = "Seller,Customer")]
        [HttpPost]
        public IActionResult EditReview(int productId, Review editedReview)
        {

            string userId = _userManager.GetUserId(User);
            var review = _context.Review.SingleOrDefault(r => r.ProductID == productId && r.CustomerId == userId);
            if (review == null)
            {
                return RedirectToAction("ProductDetail", "Product", new { id = productId });
            }

            review.Rating = editedReview.Rating;
            review.Comment = editedReview.Comment;
            _context.SaveChanges();



            return RedirectToAction("ProductDetail", "Product", new { id = productId });
        }

        [Authorize(Roles = "Seller,Customer")]
        [HttpPost]
        public IActionResult DeleteReview(int productId)
        {

            string userId = _userManager.GetUserId(User);
            var review = _context.Review.SingleOrDefault(r => r.ProductID == productId && r.CustomerId == userId);
            if (review == null)
            {
                return RedirectToAction("ProductDetail", "Product", new { id = productId });
            }

            _context.Review.Remove(review);
            _context.SaveChanges();

            return RedirectToAction("ProductDetail", "Product", new { id = productId });
        }

    }
}