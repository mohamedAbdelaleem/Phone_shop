using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            Order order = cart.CreateOrder();
            order.PickupAddressId = ViewModel.Address.AddressId;
            _db.Order.Add(order);
            _db.PickupAddress.Add(ViewModel.Address);
            _db.SaveChanges();
            await _db.SaveChangesAsync();
            return RedirectToAction("Confirmation",cart.ShoppingCartId);
        }
        ViewModel.Governorates = _db.Governorates.ToList();
        ViewModel.Cities = _db.Cities.ToList();
        return View(ViewModel);
    }
    [HttpGet]
    public IActionResult GetCities(int governorateId)
    {
        var cities = _db.Cities.Where(c=>c.governorate_id==governorateId);
        return Json(cities);
    }
    [HttpPost]
    public IActionResult Confirmation(string UserId)
    {
        _db.ShoppingCartItems.RemoveRange(_db.ShoppingCartItems.Where(ci => ci.CartId == UserId));
        _db.SaveChanges();
        var cart = ShoppingCart.GetCart(this.HttpContext, _db);
        cart.EmptyCart();
        return View();
    }
}
