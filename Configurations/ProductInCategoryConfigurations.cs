using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class ProductInCategoryConfigurations : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.HasNoKey();
            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.ToTable("ProductInCategory");

            builder.HasOne(x => x.GetProducts).WithMany(x => x.GetsProductInCategories).HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.GetCategory).WithMany(x => x.GetsProductInCategories).HasForeignKey(x =>x.CategoryId);
        }
    }
}
