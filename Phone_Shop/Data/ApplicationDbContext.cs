using System.Diagnostics.Metrics;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Models;

namespace Phone_Shop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<PickupAddress> PickupAddress { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Review> Review { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderID, o.ProductID });
            modelBuilder.UseCollation("Arabic_CI_AS"); // Set collation if needed
            modelBuilder.Entity<City>().Property(e => e.city_name_ar).IsUnicode(true);
            modelBuilder.Entity<Governorate>().Property(e => e.governorate_name_ar).IsUnicode(true);
            modelBuilder.Entity<Account>()
           .HasOne(A => A.User)
           .WithOne()
           .HasForeignKey<Account>(A => A.Id)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductID)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany()
            .HasForeignKey(oi => oi.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.PickupAddress)
            .WithOne()
            .HasForeignKey<Order>(o => o.PickupAddressId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PickupAddress>()
            .HasOne(pi => pi.User)
            .WithMany()
            .HasForeignKey(pi => pi.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                    .HasOne(s => s.Seller)
                    .WithMany()
                    .HasForeignKey(s => s.SellerId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithMany()
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItem>()
            .HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
           .HasOne(o => o.Governorate)
           .WithMany()
           .HasForeignKey(o => o.governorate_id)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>()
                .HasKey(r => new { r.CustomerId, r.ProductID });

            modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany()
            .HasForeignKey(r => r.ProductID)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Review>()
            .HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}