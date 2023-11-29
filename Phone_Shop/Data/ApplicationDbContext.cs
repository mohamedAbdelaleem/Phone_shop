using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Phone_Shop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Phone_Shop.Models.Category> Category { get; set; } = default!;
        public DbSet<Phone_Shop.Models.Seller> Seller { get; set; } = default!;
        public DbSet<Phone_Shop.Models.Product> Product { get; set; } = default!;
    }
}