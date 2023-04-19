using DoAn.Repositories.Users;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        public readonly IUserRepositories _userRepositories;
        public AuthController(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> login(UserLogin u)
        {
            var result = await _userRepositories.Login(u);
            if(result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("/register")]
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
