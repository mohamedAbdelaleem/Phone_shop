using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Delivery")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeliveryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Home(bool Checked=false)
        {
            var Result = _context.Order;
            if (Checked)
                return View(Result.Where(o => o.Status == "Checked"));
            else
                return View(Result.Where(o => o.Status == "UnChecked"));

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

        [HttpPost]
        public IActionResult ChangeStatusToChecked(int id)
        {
            var order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null || order.Status== "Checked" || order.Status == "delivered")
            {
                return RedirectToAction("Home", "Delivery");
            }

            order.Status = "Checked";
            _context.SaveChanges();

            return RedirectToAction("Home", "Delivery");

        }

        [HttpPost]
        public IActionResult ChangeStatusTodelivered(int id)
        {
            var order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null || order.Status == "delivered" || order.Status == "UnChecked")
            {
                return RedirectToAction("Home", "Delivery");
            }

            order.Status = "delivered";
            _context.SaveChanges();

            return RedirectToAction("Home", "Delivery");

        }

    }
}
