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
        public DbSet<ProductAddress> ProductAddress { get; set; }
        public DbSet<Account> Account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderID, o.ProductID });

            modelBuilder.Entity<Account>()
           .HasOne(A => A.User)
           .WithOne()
           .HasForeignKey<Account>(A => A.Id)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.ProductID)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithOne()
            .HasForeignKey<OrderItem>(oi => oi.OrderID)
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
            .WithOne()
            .HasForeignKey<PickupAddress>(pi => pi.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductAddress>()
                    .HasOne(pa => pa.Product)
                    .WithOne(p => p.ProductAddress)
                    .HasForeignKey<ProductAddress>(pa => pa.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Seller)
            .WithOne()
            .HasForeignKey<Product>(p => p.SellerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}