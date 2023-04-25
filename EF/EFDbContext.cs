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

            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens").HasKey(x => x.UserId);
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> categories { get; set; } 

        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductInCategory> GetsProductInCategory { get; set; }


    }
}
