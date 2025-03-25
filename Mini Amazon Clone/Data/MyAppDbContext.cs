using Microsoft.EntityFrameworkCore;
using Mini_Amazon_Clone.Models;

namespace Mini_Amazon_Clone.Data
{
    public class MyAppDbContext:DbContext
    {

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // User - Product (One to Many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Admin)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            // User - Order (One to Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            // Order - OrderItems (One-to-Many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            // Product - OrderItems (One-to-Many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //// Seed Users
            //modelBuilder.Entity<User>().HasData(
            //    new User { UserID = 1, Name = "Admin User", Email = "admin@example.com", Password = "admin123", Role = "Admin" },
            //    new User { UserID = 2, Name = "John Doe", Email = "john@example.com", Password = "password123", Role = "Customer" }
            //);

            //// Seed Products
            //modelBuilder.Entity<Product>().HasData(
            //    new Product { ProductID = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1200.99m, Stock = 10, CreatedBy = 1 },
            //    new Product { ProductID = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 799.49m, Stock = 15, CreatedBy = 1 }
            //);

            //// Seed Orders
            //modelBuilder.Entity<Order>().HasData(
            //    new Order { OrderID = 1, UserID = 2, OrderDate = new DateTime(2024, 3, 23, 12, 0, 0), TotalAmount = 2000.48m, Status = "Completed" }
            //);

            //// Seed OrderItems
            //modelBuilder.Entity<OrderItem>().HasData(
            //    new OrderItem { OrderItemID = 1, OrderID = 1, ProductID = 1, Quantity = 1, Price = 1200.99m },
            //    new OrderItem { OrderItemID = 2, OrderID = 1, ProductID = 2, Quantity = 1, Price = 799.49m }
            //);



        }



    }
}
