using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Delivery")]
    public class DeliveryController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
