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

        [HttpGet("getAllUser")]
        public async Task<IActionResult> getAllUser([FromQuery]GetAllUser getAll)
        {
            return Ok(await _userRepositories.getAllUser(getAll));
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

            var result = await _userRepositories.GetMe();
            if (result.IsSuccessed)
            {
                return Ok(result);

            }
            return BadRequest(result);

        }

        [HttpPost("lockUser")]
        public async Task<IActionResult> lockUser(string id, DateTime? dateTime)
        {
            var result = await _userRepositories.LockUser(id, dateTime);
            if(result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("unlockUser")]
        public async Task<IActionResult> unlockUser(string id)
        {
            var result = await _userRepositories.UnLockUser(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("decentralization")]
        public async Task<IActionResult> decentralization(string id, string type)
        {
            var result = await _userRepositories.decentralization(id, type);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody]string id)
        {
            var result = await _userRepositories.Delete(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
          
        }
    }
}
