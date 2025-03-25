using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Amazon_Clone.Controllers;
using Mini_Amazon_Clone.Data;
using Mini_Amazon_Clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mini_Amazon_Clone.Tests
{
    public class OrderControllerTests
    {
        private readonly DbContextOptions<MyAppDbContext> _options;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            // Create in-memory database options
            _options = new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create actual context for testing
            var context = new MyAppDbContext(_options);

            // Initialize the controller with real context
            _controller = new OrderController(context);
        }

        [Fact]
        public void CreateOrder_ReturnsOk_WhenOrderIsValid()
        {
            // Arrange
            var newOrder = new Order
            {
                UserID = 1,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductID = 1, Quantity = 2 }
                },
                TotalAmount = 100.00m
            };

            // Act
            var result = _controller.CreateOrder(newOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Verify the order was saved to the database
            using (var context = new MyAppDbContext(_options))
            {
                var savedOrder = context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault();

                Assert.NotNull(savedOrder);
                Assert.Equal("Pending", savedOrder.Status);
                Assert.True((DateTime.UtcNow - savedOrder.OrderDate).TotalSeconds < 5);
                Assert.Single(savedOrder.OrderItems);
            }
        }

        //[Fact]
        //public async Task GetOrdersByUserId_ReturnsOrdersWithItems_WhenUserHasOrders()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var testOrders = new List<Order>
        //    {
        //        new Order
        //        {
        //            OrderID = 1,
        //            UserID = userId,
        //            Status = "Pending",
        //            OrderItems = new List<OrderItem>
        //            {
        //                new OrderItem { OrderItemID = 1, ProductID = 101, Quantity = 2 },
        //                new OrderItem { OrderItemID = 2, ProductID = 102, Quantity = 1 }
        //            }
        //        },
        //        new Order
        //        {
        //            OrderID = 2,
        //            UserID = userId,
        //            Status = "Completed",
        //            OrderItems = new List<OrderItem>
        //            {
        //                new OrderItem { OrderItemID = 3, ProductID = 103, Quantity = 3 }
        //            }
        //        }
        //    };

        //    using (var context = new MyAppDbContext(_options))
        //    {
        //        await context.Orders.AddRangeAsync(testOrders);
        //        await context.SaveChangesAsync();
        //    }

        //    // Act
        //    var result = _controller.GetOrdersByUserId(userId);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var returnedOrders = Assert.IsAssignableFrom<List<Order>>(okResult.Value);

        //    Assert.Equal(2, returnedOrders.Count);
        //    Assert.All(returnedOrders, o =>
        //    {
        //        Assert.Equal(userId, o.UserID);
        //        Assert.NotNull(o.OrderItems);
        //        Assert.NotEmpty(o.OrderItems);
        //    });
        //}


        [Fact]
        public async Task GetOrdersByUserId_ReturnsUsersOrders()
        {
            // Arrange
            var userId1 = 1;
            var userId2 = 2;

            using (var context = new MyAppDbContext(_options))
            {
                await context.Orders.AddRangeAsync(
                    new Order { OrderID = 1, UserID = userId1, Status = "Pending" },
                    new Order { OrderID = 2, UserID = userId1, Status = "Completed" },
                    new Order { OrderID = 3, UserID = userId2, Status = "Pending" }
                );
                await context.SaveChangesAsync();
            }

            // Act
            var result = _controller.GetOrdersByUserId(userId1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrders = Assert.IsAssignableFrom<List<Order>>(okResult.Value);

            Assert.Equal(2, returnedOrders.Count);
            Assert.All(returnedOrders, o => Assert.Equal(userId1, o.UserID));
            Assert.DoesNotContain(returnedOrders, o => o.UserID == userId2);
        }
    }
}