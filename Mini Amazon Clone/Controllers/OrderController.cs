using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Amazon_Clone.Data;
using Mini_Amazon_Clone.Models;

namespace Mini_Amazon_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly MyAppDbContext _context;

        public OrderController(MyAppDbContext context)
        {
            _context = context;
        }


        [Authorize(Policy = "CanViewOrdersPolicy")]
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }







        [Authorize(Policy = "CanRefundOrdersPolicy")]
        [HttpPost("refund/{orderId}")]
        public IActionResult RefundOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

           // Make order refunded 
            order.Status = "Refunded";
            _context.SaveChanges();

            return Ok($"Order {orderId} refunded successfully.");
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order request. Order must contain at least one product.");
            }

            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending"; // Set the status to "Pending"

            // Add the order and its items to the database
            _context.Orders.Add(order);
            _context.SaveChanges(); // Save to DB

            return Ok(new { message = "Order placed successfully.", orderId = order.OrderID });
        }



        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders
                .Where(o => o.OrderID == id)
                .Select(o => new
                {
                    o.OrderID,
                    o.UserID,
                    o.OrderDate,
                    o.Status,
                    o.TotalAmount
                })
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var orders = _context.Orders
                                 .Where(o => o.UserID == userId)
                                 .Include(o => o.OrderItems) 
                                 .ToList();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for this user.");
            }

            return Ok(orders);
        }






    }
}
