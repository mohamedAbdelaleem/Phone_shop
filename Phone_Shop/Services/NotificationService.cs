using Microsoft.CodeAnalysis;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Services
{

    public interface INotification
    {

        public void SendProductAccepted(string userId, int productId);
        public void SendProductRejected(string userId, int productId);

        public void SendOrderShipped(string userId, int orderId);
        public void SendOrderDelivered(string userId, int orderId);

    }

    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        public NotificationService(ApplicationDbContext context) {
            
            _context = context;
        }

        public void SendProductAccepted(string userId, int productId) {
            
            var content = $"Your Product Application for {productId} has been Accepted!";
            var notification = new Notification { UserId=userId, Content=content, IsReaded=false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();

            return;
        }
        public void SendProductRejected(string userId, int productId)
        {
            var content = $"Your Product Application for {productId} has been Rejected!";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public void SendOrderShipped(string userId, int orderId)
        {
            var content = $"Your Order (#{orderId}) has been Shipped!";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }
        public void SendOrderDelivered(string userId, int orderId)
        {
            var content = $"Your Order (#{orderId}) has been Delivered!";
            var notification = new Notification { UserId = userId, Content = content, IsReaded = false };
            notification.CreatedAt = DateTime.Now;

            _context.Notification.Add(notification);
            _context.SaveChanges();
        }


    }
}
