using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Services
{

    public interface INotification
    {

        public void SendProductAccepted(string userId, string productName);
        public void SendProductRejected(string userId, string productName);

        public void SendOrderShipped(string userId, int orderId);
        public void SendOrderDelivered(string userId, int orderId);

        public int CountUnReadNotifications(string userId);

    }

    public class NotificationService: INotification
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public NotificationService(ApplicationDbContext context, UserManager<IdentityUser> UserManager) {
            
            _context = context;
            _userManager = UserManager;
        }

        public void SendProductAccepted(string userId, string productName) {
            
            var content = $"Your Product Application for ({productName}) has been Accepted.";
            var notification = new Notification { UserId=userId, Content=content, IsReaded=false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();

            return;
        }
        public void SendProductRejected(string userId, string productName)
        {
            var content = $"Unfortunately, Your Product Application for ({productName}) has been Rejected.";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public void SendOrderShipped(string userId, int orderId)
        {
            var content = $"Your Order number #{orderId} has been Shipped!";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }
        public void SendOrderDelivered(string userId, int orderId)
        {
            var content = $"Your Order number #{orderId} has been Delivered";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public int CountUnReadNotifications(string userId)
        {
            
            var notificationCount = _context.Notification.Where(n => n.UserId == userId && !n.IsReaded).Count();
            return notificationCount;

        }
    }
}
