using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Amazon_Clone.Data;
using Mini_Amazon_Clone.Models;

namespace Mini_Amazon_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly MyAppDbContext _context;

        public ProductController(MyAppDbContext context)
        {
            _context = context;
        }


        [HttpGet("all")]
        public IActionResult GetAllProduct()
        {
            var products = _context.Products.ToList();
            return Ok(products);

        }



        [Authorize(Roles ="Admin")]
        [HttpPost("add")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest("Invalid product data.");
            }
     
            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok("Product Addedd Successfully");
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct([FromRoute]int id , [FromBody] Product updatedProduct)
        {
            var product = _context.Products.Find(id);
            if(product == null)
            {
                return NotFound("Product not found.");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.Description = updatedProduct.Description;

            _context.Products.Update(product); 
            _context.SaveChanges();


            return Ok("Product updated Successfully");
        }
    }
}
