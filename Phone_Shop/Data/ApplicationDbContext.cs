using System.Diagnostics.Metrics;
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
        public DbSet<Seller> Seller { get; set; }
        public DbSet<Product> Product { get; set; } 
        public DbSet<Order> Oder { get; set; } 
        public DbSet<OrderItem> OrderItem { get; set; } 
        public DbSet<Address> Address { get; set; } 
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
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.OrderID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.PickupAddress)
                .WithOne(pa => pa.Address)
                .HasForeignKey<PickupAddress>(pa => pa.address_id);

            modelBuilder.Entity<Address>()
                    .HasOne(a => a.ProductAddress)
                    .WithOne(pa => pa.Address)
                    .HasForeignKey<ProductAddress>(pa => pa.address_id);

            modelBuilder.Entity<Product>()
                    .HasOne(p => p.ProductAddress)
                    .WithOne(pa => pa.Product)
                    .HasForeignKey<ProductAddress>(pa => pa.product_id);

        }
    }
}