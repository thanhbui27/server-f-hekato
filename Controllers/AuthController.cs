using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.Repositories.Users;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class AuthController : Controller
    {
        public readonly IUserRepositories _userRepositories;
        public readonly UserManager<UserModels> _userManager;
        private readonly SignInManager<UserModels> _signInManager;

        public AuthController(IUserRepositories userRepositories, UserManager<UserModels> userManager, SignInManager<UserModels> signInManager)
        {
            _userRepositories = userRepositories;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [AllowAnonymous]
        [HttpGet("/google-signin")]
        public async Task<IActionResult> LoginWithGoogle()
        {
            var callbackUrl = Url.Action("ProviderResponse", "Auth", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/Auth/ProviderResponse")]
        public async Task<IActionResult> ProviderResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Redirect("http://localhost:3000/setAuth?error=info_not_exits");
            }
            
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));

                var token = _userRepositories.CreateToken(user.Email, user.Id.ToString(), user.fullName, user.UserName, user.type);
                return Redirect("http://localhost:3000/setAuth?token=" + token);


            }
            if (result.IsLockedOut)
            {
                return Redirect("http://localhost:3000/setAuth?error=account_is_lock");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstName = info.Principal.FindFirst(ClaimTypes.GivenName)?.Value;
                var lastName = info.Principal.FindFirst(ClaimTypes.Surname)?.Value;
                if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                {
                    return Redirect("http://localhost:3000/setAuth?error=info_not_exits");
                }
                if (string.IsNullOrEmpty(email))
                {
                    Random random = new Random();
                    int randomMilliseconds = random.Next(1000000);
                    email = "detault_account_"+ randomMilliseconds+"@gmail.com";
                }
                var user = new UserModels
                {
                    fullName = firstName + " " + lastName,
                    UserName = email,
                    Email = email,
                    type = "user",
                };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    _userRepositories.createSession(user.Id);
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        var token = _userRepositories.CreateToken(user.Email, user.Id.ToString(), user.fullName, user.UserName, user.type);

                        return Redirect("http://localhost:3000/setAuth?token=" + token);
                    }
                }else
                {
                    return Redirect("http://localhost:3000/setAuth?error=create_account_failed");
                }
                return Redirect("http://localhost:3000/setAuth?error=error_please_try_a_gain");
            }

        }

        [AllowAnonymous]
        [HttpGet("/facebook-signin")]
        public async Task<IActionResult> LoginWithFacebook()
        {
            var callbackUrl = Url.Action("ProviderResponse", "Auth", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", callbackUrl);
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
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
