using DoAn.EF;
using DoAn.Models;
using DoAn.Repositories.Carts;
using DoAn.Repositories.Categorys;
using DoAn.Repositories.Order;
using DoAn.Repositories.ProductAction;
using DoAn.Repositories.Products;
using DoAn.Repositories.StorageService;
using DoAn.Repositories.StorageService.StorageService;
using DoAn.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
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
            builder.Services.AddAutoMapper(typeof(Program));

            string issuer = builder.Configuration["JWT:ValidIssuer"];
            string signingKey = builder.Configuration["JWT:Sercet"];
   
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);

            builder.Services.AddAuthentication(option =>
            {
                
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
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