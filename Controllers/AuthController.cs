using DoAn.Repositories.Users;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class AuthController : Controller
    {
        public readonly IUserRepositories _userRepositories;
  

        public AuthController(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
   
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null)
            //{
            //    return NotFound();
            //}
          
            return Ok(await _userRepositories.GetMe());
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserLogin u)
        {
            var result = await _userRepositories.Login(u);
            if(result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody]UserRegister u)
        {
            var result = await _userRepositories.Register(u);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin, user")]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if (!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
