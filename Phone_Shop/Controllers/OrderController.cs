using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Phone_Shop.ViewModel;
using System;
using System.Linq;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _db;

    public OrderController(ApplicationDbContext db)
    {
        _db = db;
    }
    [Authorize]
    public ActionResult Index()
    {
        var ViewModel = new CheckoutViewModel
        {
            Governorates = _db.Governorates.ToList(),
            Cities = _db.Cities.ToList(),
            Address=new PickupAddress()
        };
        return View(ViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Index(CheckoutViewModel ViewModel)
    {
        if (ModelState.IsValid)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _db);
            var user = _db.Users.SingleOrDefault(u => u.Email == cart.ShoppingCartId);
            ViewModel.Address.UserId = user.Id;
            ViewModel.Address.User = user;
            _db.PickupAddress.Add(ViewModel.Address);
            _db.SaveChanges();
            Order order = new Order
            {
                UserId = user.Id,
                OrderedAt = DateTime.Now,
                PickupAddressId=ViewModel.Address.AddressId,
                Status= "UnChecked",
                PickupAddress=ViewModel.Address,
                User=user
            };
            _db.Order.Add(order);
            _db.SaveChanges();
            cart.CreateOrder(order);
            order.PickupAddressId = ViewModel.Address.AddressId;
            _db.ShoppingCartItems.RemoveRange(_db.ShoppingCartItems.Where(ci => ci.CartId == cart.ShoppingCartId));
            await UpdateProductQuantities(order.Id);
            await _db.SaveChangesAsync();
            return RedirectToAction("Confirmation");
        }
        ViewModel.Governorates = _db.Governorates.ToList();
        ViewModel.Cities = _db.Cities.ToList();
        return View(ViewModel);
    }
    private async Task UpdateProductQuantities(int id)
    {
        List<OrderItem> orderItems = _db.OrderItem.Where(ot => ot.OrderID == id).ToList();
        foreach (var orderItem in orderItems)
        {
            var product = _db.Product.Find(orderItem.ProductID);

            if (product != null)
            {
                if (product.Amount >= orderItem.Quantity)
                {
                    product.Amount -= orderItem.Quantity;
                }
                else
                {
                    throw new InvalidOperationException($"Insufficient quantity available for product {product.Name}.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Product with ID {orderItem.ProductID} not found.");
            }
        }

        await _db.SaveChangesAsync();
    }
[HttpGet]
    public IActionResult GetCities(int governorateId)
    {
        var cities = _db.Cities.Where(c=>c.governorate_id==governorateId);
        return Json(cities);
    }
    [HttpGet]
    public IActionResult Confirmation()
    {
        return View();
    }
}
