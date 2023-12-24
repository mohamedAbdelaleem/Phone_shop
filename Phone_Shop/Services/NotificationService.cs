using Phone_Shop.Data;

namespace Phone_Shop.Services
{

    public interface INotification
    {

        void SendProductAccepted(string userId, int productId);
        void SendProductRejected(string userId, int productId);

        void SendOrderShipped(string userId, int orderId);
        void SendOrderDelivered(string userId, int orderId);

    }

    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        public NotificationService(ApplicationDbContext context) {
            
            _context = context;
        }
    }
}
