using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Phone_Shop.Services;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Delivery")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotification _notificationService;
        public DeliveryController(ApplicationDbContext context, INotification notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        [HttpGet]
        public IActionResult Home(int stage = 0,
            DateTime? startDate = null, DateTime? endDate = null, int? governorateId = null, int? cityId = null)
        {
            ViewData["Governorates"] = _context.Governorates.ToList();
            ViewData["stage"] = stage;
            var query = _context.Order.AsQueryable();
            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderedAt <= endDate.Value);
            }

            if (governorateId.HasValue)
            {
                query = query.Where(o => o.PickupAddress.GovernorateId == governorateId.Value);
            }

            if (cityId.HasValue)
            {
                query = query.Where(o => o.PickupAddress.CityId == cityId.Value);
            }

            if (stage==0)
            {
                query = query.Where(o => o.Status == "UnShipped");
            }
            else if(stage==1)
            {
                query = query.Where(o => o.Status == "Shipped");
            }
            else
            {
                query = query.Where(o => o.Status == "delivered");
            }

            var result = query.ToList();
            return View(result);
        }

        public IActionResult OrderDetails(int id)
        {

            var order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null)
            {
                return RedirectToAction("Home", "Delivery");
            }

            var account = _context.Account.SingleOrDefault(a => a.Id == order.UserId);
            var PickupAddress = _context.PickupAddress.SingleOrDefault(p => p.AddressId == order.PickupAddressId);

            ViewData["order"] = order;
            ViewData["account"] = account;
            ViewData["PhoneNumber"] = _context.Users.SingleOrDefault(u=>u.Id== account.Id).PhoneNumber;
            ViewData["PickupAddress"] = PickupAddress;
            ViewData["Governorate"] = _context.Governorates.SingleOrDefault(g=>g.Id==PickupAddress.GovernorateId).governorate_name_en;
            ViewData["City"] = _context.Cities.SingleOrDefault(c=>c.Id==PickupAddress.CityId).city_name_en;
            ViewData["TotalPrice"] = _context.OrderItem.Where(oi => oi.OrderID==id).Select(oi=>oi.UnitPrice*oi.Quantity).Sum();
            var orderitem = _context.OrderItem.Where(oi => oi.OrderID == id).Select(oi=>oi.ProductID).ToList();
            return View("OrderDetails",_context.Product.Where(p=>orderitem.Contains(p.Id)));
        }

        public IActionResult StoreDetails(int id)
        {

            var store = _context.Store.SingleOrDefault(s => s.Id == id);
            if (store == null)
            {
                return RedirectToAction("Home", "Delivery");
            }
            return View(store);
        }


        [HttpPost]
        public IActionResult ChangeStatusToShipped(int id)
        {
            var order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null || order.Status== "Shipped" || order.Status == "delivered")
            {
                return RedirectToAction("Home", "Delivery");
            }

            order.Status = "Shipped";
            _context.SaveChanges();

            _notificationService.SendOrderShipped(order.UserId, order.Id);

            return RedirectToAction("Home", "Delivery");

        }

        [HttpPost]
        public IActionResult ChangeStatusTodelivered(int id)
        {
            var order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null || order.Status == "delivered" || order.Status == "UnShipped")
            {
                return RedirectToAction("Home", "Delivery");
            }

            order.Status = "delivered";
            _context.SaveChanges();

            _notificationService.SendOrderDelivered(order.UserId, order.Id);

            return RedirectToAction ("Home", "Delivery");

        }
        [HttpGet]
        public IActionResult GetCities(int governorateId)
        {
            var cities = _context.Cities.Where(c => c.governorate_id == governorateId);
            return Json(cities);
        }

    }
}
