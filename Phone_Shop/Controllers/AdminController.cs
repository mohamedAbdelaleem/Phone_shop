using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Phone_Shop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
