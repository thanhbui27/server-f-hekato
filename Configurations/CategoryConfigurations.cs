using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.CategoryId).UseIdentityColumn();

            builder.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
        }
    }
}
