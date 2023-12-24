using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;
using Phone_Shop.Models;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore.Storage;

namespace Phone_Shop.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;




     
        public AccountsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public IActionResult profile()
        {
            decimal totalprice= 0l;
             int totalsoldproducts = 0;
            var Userid = _userManager.GetUserId(User);
            var result = _context.Account.Where(p => p.Id == Userid).ToList();
            ViewData["PhoneNumber"]=_context.Users.SingleOrDefault(u=>(u.Id==Userid)).PhoneNumber;
             var productstoseller=_context.Product.Where(x=>x.SellerId==Userid).ToList();
             var items=_context.OrderItem.ToList();

            var test = from prodect in productstoseller
                       join item in items on prodect.Id equals item.ProductID

                       select new { item.UnitPrice, item.Quantity };
            
           foreach (var item in test)
            {
               // Console.WriteLine(item);
                totalprice+= item.UnitPrice*item.Quantity;
                totalsoldproducts += item.Quantity;

            }

            ViewData["Total Revenu"]=totalprice;
            ViewData["Total Sold Products"]=totalsoldproducts;

            return View(result);
        }


   


        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Account.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Photo")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", account.Id);
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityUser user,string name,IFormFile photo)
        {
            Account account = new Account();
            if (photo != null && photo.Length > 0)
            {
                // Get the wwwroot path
                string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "imj");
                Directory.CreateDirectory(uploadPath);

                // Generate a unique filename for the uploaded file
                string newfilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";
                string fileName = Path.Combine(uploadPath, newfilename);

                // Save the file to the server
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                account.Photo = $"/imj/{newfilename}";
            }
            else
                account.Photo = "/imj/defult.jpg";
            // Optionally, save the file path to a database or return it to the user
            account.Id = user.Id;
            account.Name = name;
            account.Email = user.Email;
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", account.Id);
            return View(account);
        }
        // GET: Accounts/Edit/5
        
        public async Task<IActionResult> Edit(string id)

        {

            var account = await _context.Account.FindAsync(id);

            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            if (account == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", account.Id);
            return View(account);
        }
        
        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,Photo,Phone")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
        //        }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", account.Id);
            return View(account);
        }





        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
            return (_context.Account?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
