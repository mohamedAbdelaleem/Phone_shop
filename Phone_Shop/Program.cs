using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Data;

namespace Phone_Shop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

            });
            //

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();


            using (var scope = app.Services.CreateScope())
            {
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Customer", "Seller", "Admin", "Delivery" };
                
                foreach (var role in roles)
                {
                    if (!await roleManger.RoleExistsAsync(role))
                    {
                        await roleManger.CreateAsync(new IdentityRole(role));
                    }
                }

            }

            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var email = "aliadmin12@phone.com";
                var password = "Ali1212#";

                if (await userManger.FindByEmailAsync(email) == null)
                {
                    var admin = new IdentityUser { Email = email, UserName = email };

                    await userManger.CreateAsync(admin, password);

                    await userManger.AddToRoleAsync(admin, "Admin");

                }
                

            }

            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var email = "delivery@phone.com";
                var password = "Ali1212#";

                if (await userManger.FindByEmailAsync(email) == null)
                {
                    var delivery = new IdentityUser { Email = email, UserName = email };

                    await userManger.CreateAsync(delivery, password);

                    await userManger.AddToRoleAsync(delivery, "Delivery");

                }


            }

            app.Run();
        }
    }
}