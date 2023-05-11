using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.Pagination;
using DoAn.Migrations;
using DoAn.Models;
using DoAn.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed= true,
                    Message = "Xoá user thành công"
                };
            }
            return new ApiErrorResult<bool>("Xoá user không thành công");
        }

        public async Task<PagedResult<UserModels>> getAllUser(GetAllUser getAll)
        {
            var user = _context.Users.ToList();

            int totalRow = await _context.Users.CountAsync();

            if (!string.IsNullOrEmpty(getAll.p))
            {
                user = (List<UserModels>)user.Where(x => x.fullName.Contains(getAll.p));
            }

            var data = user.Skip((getAll.PageIndex - 1) * getAll.PageSize).Take(getAll.PageSize).ToList();

            return new PagedResult<UserModels>
            {
                Items = data,
                PageSize = getAll.PageSize,
                PageIndex = getAll.PageIndex,
                TotalRecords = totalRow
            };

        }

        public async Task<ApiResult<bool>> LockUser(string id, DateTime? endDate)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
                    user.LockoutEnd = endDate;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Khoá user thành công"
                    };
                }
            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message.ToString()
                };

            }
            return new ApiErrorResult<bool>
            {
                IsSuccessed = false,
                Message = "Có lỗi xảy ra vui lòng thử lại"
            };


        }

        public async Task<ApiResult<bool>> UnLockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.LockoutEnd = DateTime.Now;
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Mở khoá user thành công"
                };
            }

            return new ApiErrorResult<bool>
            {
                IsSuccessed = false,
                Message = "Có lỗi xảy ra vui lòng thử lại"
            };

        }

        public async Task<ApiResult<bool>> decentralization(string id, string type)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if(type == "admin" || type == "user")
                {
                    user.type = type;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Phân quyền thành công"
                    };
                }else
                {
                    return new ApiErrorResult<bool>
                    {
                        IsSuccessed = false,
                        Message = "Loại quyền không đúng định dạng"
                    };
                }
             
            }
            return new ApiErrorResult<bool>
            {
                IsSuccessed = false,
                Message = "Có lỗi xảy ra vui lòng thử lại"
            };
        }
    }
}
