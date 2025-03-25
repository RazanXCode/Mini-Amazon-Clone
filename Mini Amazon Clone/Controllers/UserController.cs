using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mini_Amazon_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [Authorize] 
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
          
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userName == null)
            {
                return Unauthorized("User not found.");
            }

            return Ok(new
            {
                UserName = userName,
                Role = userRole
            });
        }
    }
}
