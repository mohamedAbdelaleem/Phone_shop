// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Phone_Shop.Data;

namespace Phone_Shop.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private  ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IndexModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager; 
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            [StringLength(11, MinimumLength = 11)]
            public string PhoneNumber { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            [Display(Name = "Photo")]
            public IFormFile ? Photo { get; set; }
        }



        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            var account =  _context.Account.Single(x => x.Id == userId); 


            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = account.Name,
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            var account = _context.Account.Single(x => x.Id == userId);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.Name != account.Name)
            {
                account.Name = Input.Name; 
                _context.SaveChanges();
            }

            if (Input.Photo != null && Input.Photo.Length > 0)
            {
                // Get the wwwroot path
                string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "imj");
                Directory.CreateDirectory(uploadPath);

                // Generate a unique filename for the uploaded file
                string newfilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(Input.Photo.FileName)}";
                string fileName = Path.Combine(uploadPath, newfilename);

                // Save the file to the server
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    Input.Photo.CopyTo(fileStream);
                }
                account.Photo = $"/imj/{newfilename}";
                _context.SaveChanges();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
