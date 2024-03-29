﻿using Microsoft.AspNetCore.Authorization;
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
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Authorize]
    public ActionResult Index()
    {
        var ViewModel = new CheckoutViewModel
        {
            Governorates = _context.Governorates.ToList(),
            Cities = _context.Cities.ToList(),
            Address=new PickupAddress()
        };
        return View(ViewModel);
    }

    public ActionResult OrderHistory(string Id)
    {
        var orders = _context.Order.Where(o => o.UserId == Id);
        return View(orders);
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
        ViewData["PhoneNumber"] = _context.Users.SingleOrDefault(u => u.Id == account.Id).PhoneNumber;
        ViewData["PickupAddress"] = PickupAddress;
        ViewData["Governorate"] = _context.Governorates.SingleOrDefault(g => g.Id == PickupAddress.GovernorateId).governorate_name_en;
        ViewData["City"] = _context.Cities.SingleOrDefault(c => c.Id == PickupAddress.CityId).city_name_en;
        ViewData["TotalPrice"] = _context.OrderItem.Where(oi => oi.OrderID == id).Select(oi => oi.UnitPrice * oi.Quantity).Sum();
        var orderitem = _context.OrderItem.Where(oi => oi.OrderID == id).Select(oi => oi.ProductID).ToList();
        return View("OrderDetails", _context.Product.Where(p => orderitem.Contains(p.Id)));
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
    public async Task<IActionResult> Index(CheckoutViewModel ViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == ShoppingCart.ShoppingCartId);
            ViewModel.Address.UserId = user.Id;
            ViewModel.Address.User = user;
            ViewModel.Address.Governorate = _context.Governorates.SingleOrDefault(g => g.Id == ViewModel.Address.GovernorateId);
            ViewModel.Address.City = _context.Cities.SingleOrDefault(c => c.Id == ViewModel.Address.CityId);
            ViewModel.Address.User = user;
            _context.PickupAddress.Add(ViewModel.Address);
            _context.SaveChanges();
            Order order = new Order
            {
                UserId = user.Id,
                OrderedAt = DateTime.Now,
                PickupAddressId=ViewModel.Address.AddressId,
                Status= "UnShipped",
                PickupAddress=ViewModel.Address,
                User=user
            };
            _context.Order.Add(order);
            _context.SaveChanges();
            var cart = ShoppingCart.GetCart(this.HttpContext, _context);
            cart.CreateOrder(order);
            order.PickupAddressId = ViewModel.Address.AddressId;
            _context.ShoppingCartItems.RemoveRange(_context.ShoppingCartItems.Where(ci => ci.CartId == ShoppingCart.ShoppingCartId));
            await UpdateProductQuantities(order.Id);
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirmation");
        }
        ViewModel.Governorates = _context.Governorates.ToList();
        ViewModel.Cities = _context.Cities.ToList();
        return View(ViewModel);
    }
    private async Task UpdateProductQuantities(int id)
    {
        List<OrderItem> orderItems = _context.OrderItem.Where(ot => ot.OrderID == id).ToList();
        foreach (var orderItem in orderItems)
        {
            var product = _context.Product.Find(orderItem.ProductID);

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

        await _context.SaveChangesAsync();
    }
[HttpGet]
    public IActionResult GetCities(int governorateId)
    {
        var cities = _context.Cities.Where(c=>c.governorate_id==governorateId);
        return Json(cities);
    }
    [HttpGet]
    public IActionResult Confirmation()
    {
        return View();
    }
}
