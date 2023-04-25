using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoAn.Repositories.Users
{
    public class UserRepositories : IUserRepositories
    {
        public readonly UserManager<UserModels> _userManager;
        private readonly SignInManager<UserModels> _signInManager;
        private readonly IConfiguration _config;
        public UserRepositories(UserManager<UserModels> userManager, SignInManager<UserModels> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        public async Task<ApiResult<string>> Login(UserLogin user)
        {
            var u = await _userManager.FindByNameAsync(user.userName);
            if(u == null)
            {
                return new ApiErrorResult<string>("Tài khoản này không tồn tại");
            }

            var result = await _signInManager.PasswordSignInAsync(u, user.Password, false, false);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Tài khoản hoặc mật khẩu không đúng");
            }
         
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,u.Email),
                new Claim(ClaimTypes.GivenName, u.firstName + u.lastName),
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            string issuer = _config["JWT:ValidIssuer"];
            string signingKey = _config["JWT:Sercet"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience= issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds
            };
            //var token = new JwtSecurityToken(
            //    issuer: issuer,
            //    audience: issuer,
            //    expires: DateTime.Now.AddMinutes(1),
            //    claims: claims,
            //    signingCredentials: creds
            //);

            var tokenHandle = new JwtSecurityTokenHandler();
            var crToken = tokenHandle.CreateToken(tokenDescriptor);


            return new ApiSuccessResult<string>(tokenHandle.WriteToken(crToken));

        }

        public async Task<ApiResult<bool>> Register(UserRegister u)
        {
            var user = await _userManager.FindByNameAsync(u.userName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            if (await _userManager.FindByEmailAsync(u.Email) != null)
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }

            user = new UserModels()
            {
                Email = u.Email,
                firstName = u.FirstName,
                lastName = u.LastName,
                UserName = u.userName,
                dob = DateTime.UtcNow.Date,
        };
            var result = await _userManager.CreateAsync(user, u.Password);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>("Đăng ký không thành công");
        }
    }
}
