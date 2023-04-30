using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Migrations;
using DoAn.Models;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly EFDbContext _context;
        public UserRepositories(UserManager<UserModels> userManager, SignInManager<UserModels> signInManager, IConfiguration config, IHttpContextAccessor httpContextAccessor, IMapper mapper, EFDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = dbContext;
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
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.GivenName, u.fullName),
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Role, u.type)
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
                Expires = DateTime.Now.AddHours(12),
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
        public async Task<ApiResult<UserModels>> GetMe()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            user.session = _context.session_u.Select(s => new Session
            {
                SessionId = s.SessionId,
                Uid = s.Uid
            }).Where(u => u.Uid == user.Id).FirstOrDefault();
            
            return new ApiSuccessResult<UserModels>(user);
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
                fullName = u.fullName,
                PhoneNumber = u.PhoneNumber,
                UserName = u.userName,
                dob = DateTime.UtcNow.Date,
                type = "user"           
            };

   

            var result = await _userManager.CreateAsync(user, u.Password);
            if (result.Succeeded)
            {
                _context.session_u.Add(new Session
                {
                    Uid = user.Id,
                });
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>("Đăng ký không thành công");
        }

        public async Task<ApiResult<bool>> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id); //use async find
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>();
        }
    }
}
