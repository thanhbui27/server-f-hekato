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
           
            base.OnModelCreating(modelBuilder);
        }

        
    }
}
