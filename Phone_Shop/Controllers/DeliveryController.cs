using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Phone_Shop.Services;
using System.Linq;

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
            var query = _context.Order.Include(o=>o.PickupAddress).ToList();
            if (stage == 0)
            {
                query = query.Where(q => q.Status == "UnShipped").ToList();
            }
            else if (stage == 1)
            {
                query = query.Where(q => q.Status == "Shipped").ToList();
            }
            else
            {
                query = query.Where(q => q.Status == "delivered").ToList();
            }
            if (startDate.HasValue)
            {
                query = query.Where(q => q.OrderedAt >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                query = query.Where(q => q.OrderedAt <= endDate.Value).ToList();
            }
            if (stage != 0)
            {
                if (governorateId.HasValue)
                {
                    query = query.Where(q=> q.PickupAddress.GovernorateId == governorateId.Value).ToList();
                }

                if (cityId.HasValue)
                {
                    query = query.Where(q => q.PickupAddress.CityId == cityId.Value).ToList();
                }
            }
            else if(governorateId.HasValue || cityId.HasValue)
            {
                List<Store> stores = new List<Store>();
                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (var item in query)
                {
                    var SingleOrder = _context.OrderItem.Include(o=>o.Product.Store).Where(o => o.OrderID == item.Id).ToList();
                    orderItems.AddRange(SingleOrder);
                }
                stores = orderItems.Select(o => o.Product.Store).ToList();
                if (governorateId.HasValue)
                {
                    string govName = _context.Governorates.SingleOrDefault(g => g.Id == governorateId).governorate_name_en;
                    stores = stores.Where(s => s.Governace == govName).ToList();
                }

                if (cityId.HasValue)
                {
                    string CityName = _context.Cities.SingleOrDefault(c => c.Id == cityId).city_name_en;
                    stores = stores.Where(s => s.City == CityName).ToList();
                }
                orderItems = orderItems.Where(o => stores.Any(s => o.Product.StoreId == s.Id)).ToList();
                query = query.Where(q => orderItems.Any(o => o.OrderID == q.Id)).ToList();
            }
            return View(query);
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
