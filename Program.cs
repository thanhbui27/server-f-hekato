using DoAn.EF;
using DoAn.Models;
using DoAn.Repositories.Carts;
using DoAn.Repositories.Categorys;
using DoAn.Repositories.Comment;
using DoAn.Repositories.Email;
using DoAn.Repositories.MoMo;
using DoAn.Repositories.Order;
using DoAn.Repositories.ProductAction;
using DoAn.Repositories.Products;
using DoAn.Repositories.StorageService;
using DoAn.Repositories.StorageService.StorageService;
using DoAn.Repositories.Users;
using DoAn.Repositories.VNPAY;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;

namespace DoAn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
            builder.Services.AddControllers();
            builder.Services.AddIdentity<UserModels, IdentityRole<Guid>>().AddEntityFrameworkStores<EFDbContext>().AddDefaultTokenProviders();

            //builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

          

            builder.Services.AddDbContext<EFDbContext>((options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("econnection"))));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Authorization Header",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey 
                });

                option.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            builder.Services.AddTransient<IUserRepositories, UserRepositories>();
            builder.Services.AddTransient<ICategoryRepositories, CategoryRepositories>();
            builder.Services.AddTransient<IStorageService, StorageServices>();
            builder.Services.AddTransient<IProductRepositories, ProductRepositories>();
            builder.Services.AddTransient<IProductActionRepositories, ProductActionRepositories>();
            builder.Services.AddTransient<ICartRepositories, CartRepositories>();
            builder.Services.AddTransient<IOrderRepositories, OrderRepositories>();
            builder.Services.AddTransient<ICommentsRepositories, CommentsRepositories>();
            builder.Services.AddTransient<IVnpayRepositories, VnpayRepositories>();
            builder.Services.AddTransient<IMoMoRepositories, MoMoRepositories>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddTransient<IEmailRepositories, EmailRepositories>();

            string issuer = builder.Configuration["JWT:ValidIssuer"];
            string signingKey = builder.Configuration["JWT:Sercet"];
   
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddCookie().AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = issuer,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                    ClockSkew = TimeSpan.Zero
                };
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                googleOptions.Scope.Add("profile");
                googleOptions.Events.OnCreatingTicket = (context) =>
                {
                    var picture = context.User.GetProperty("picture").GetString();

                    context.Identity.AddClaim(new Claim("picture", picture));

                    return Task.CompletedTask;
                };

            }).AddFacebook(facebookOptions => {
                //facebookOptions.AppId = "251349594049440";
                //facebookOptions.AppSecret = "7de0fe569f867fbabb9c0f97f4eb033b";
                facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.SignInScheme = IdentityConstants.ExternalScheme;
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
                facebookOptions.Events.OnCreatingTicket = (context) =>
                {
                    var picture = $"https://graph.facebook.com/{context.Principal.FindFirstValue(ClaimTypes.NameIdentifier)}/picture?type=large";
                    context.Identity.AddClaim(new Claim("picture", picture));
                    return Task.CompletedTask;
                };

            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/login";
                option.LogoutPath = "/logout";
                option.AccessDeniedPath = "/not-found";
            });

            var app = builder.Build();
            
            app.UseCors(builder =>
             builder.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod()
          );
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger f-hekato V1");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Images")),
                RequestPath = new PathString("/Resources/Images")
            });


            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();



            app.Run();
        }
    }
}