using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Data;
using Phone_Shop.Services;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Customer,Seller")]
    public class NotificationController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationController(ApplicationDbContext context,
            UserManager<IdentityUser> UserManager,
            INotification notificationService)
        {
            _context = context;
            _userManager = UserManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _context.Notification.Where(n => n.UserId == userId).ToList();

            ViewData["notifications"] = notifications;
            return View();
        }

        [HttpPost]
        public IActionResult MarkAsReaded(int id) { 
            
            var notifaction = _context.Notification.SingleOrDefault(n => n.Id == id);
            if (notifaction == null)
            {
                return NotFound();
            }
            
            notifaction.IsReaded = true;
            _context.SaveChanges();

            return RedirectToAction("Index", "Notification");

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            var notifaction = _context.Notification.SingleOrDefault(n => n.Id == id);
            if (notifaction == null)
            {
                return NotFound();
            }

            _context.Notification.Remove(notifaction);
            _context.SaveChanges();

            return RedirectToAction("Index", "Notification");

        }


    }
}
