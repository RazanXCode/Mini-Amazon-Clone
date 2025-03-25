using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mini_Amazon_Clone.Data;
using Mini_Amazon_Clone.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mini_Amazon_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly MyAppDbContext _context;

        public AuthController(IConfiguration config , MyAppDbContext context)
        {
            _config = config;
            _context = context;
        }



        // A register endpoint to allow users to sign up 
        [HttpPost("signup")]
        
        public IActionResult SignUp([FromBody] SignupDto model)
        {
            if(_context.Users.Any(U => U.Email == model.Email))
            {
                return BadRequest("Email is already registered. ");
            }

            // create a new user 
            var user = new User
            {
                Name = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role

            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");

        }











        //A login endpoint to return a JWT token upon successful authentication 
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {

            var user = _context.Users.FirstOrDefault(u => u.Name == model.UserName && u.Password == model.Password);
            if(user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Initialize role-based claims with default values
            string canViewOrders = "false"; // Claim for customers to view orders
            string canRefundOrders = "false"; // Claim for admins to do refunds


            if (user.Role == "Admin")
            {
                // Grant the "CanRefundOrders" claim to admins
                canRefundOrders = "true";
            }
            else if (user.Role == "Customer")
            {
                // Grant the "CanViewOrders" claim to customers
                canViewOrders = "true";

            }

            var token = GenerateToken(user.Name, user.Role, canViewOrders, canRefundOrders);

            return Ok(new { token });


        }

        // Token generation method 
        private string GenerateToken(string username , string role , string CanViewOrders , string CanRefundOrders) {


            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , username),
                new Claim(ClaimTypes.Role , role),
                new Claim("CanViewOrders" ,CanViewOrders ),
                new Claim("CanRefundOrders" ,CanRefundOrders )
            };

            var key = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                 issuer: jwtSettings["Issuer"],
                 audience: jwtSettings["Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                 signingCredentials: credentials
           );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
