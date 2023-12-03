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
        public DbSet<Order> Oder { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<PickupAddress> PickupAddress { get; set; }
        public DbSet<ProductAddress> ProductAddress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderID, o.ProductID });

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.ProductAddress)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.PickupAddressId)
            .WithOne()
            .HasForeignKey<PickupAddress>(pi => pi.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.UserId)
            .WithOne()
            .HasForeignKey<PickupAddress>(pi => pi.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PickupAddress>()
            .HasOne(pi => pi.UserId)
            .WithOne()
            .HasForeignKey<IdentityUser>(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                    .HasOne(p => p.ProductAddress)
                    .WithOne(pa => pa.Product)
                    .HasForeignKey<ProductAddress>(pa => pa.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasOne(s => s.Seller)
            .WithOne()
            .HasForeignKey<IdentityUser>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                  .HasMany(c => c.Products)
                  .WithOne(c => c.Category)
                  .HasForeignKey(c => c.Id)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}