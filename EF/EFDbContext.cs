using DoAn.Configurations;
using DoAn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DoAn.EF
{
    public class EFDbContext : IdentityDbContext<UserModels, IdentityRole<Guid>, Guid>
    {
        public EFDbContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfiguration(new ProductImageConfigurations());
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfigurations());
            modelBuilder.ApplyConfiguration(new ProductActionConfigurations());
            modelBuilder.ApplyConfiguration(new CartConfigurations());
            modelBuilder.ApplyConfiguration(new SessionConfigurations());
            modelBuilder.ApplyConfiguration(new OrderDetailsConfigurations());
            modelBuilder.ApplyConfiguration(new OrdersConfigurations());
            modelBuilder.ApplyConfiguration(new CommentsProductsConfigutations());



            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens").HasKey(x => x.UserId);
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> categories { get; set; } 
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductInCategory> GetsProductInCategory { get; set; }

        public DbSet<ProductActions> productActions { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Session> session_u { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<CommentsProducts> commentProducts { get; set; }
        public DbSet<Payment> payments { get; set; }

        public DbSet<IdentityUserLogin<Guid>> aspNetUserLogins { get; set; }
    }
}
