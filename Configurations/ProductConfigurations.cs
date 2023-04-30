using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));

            builder.HasKey(X => X.ProductId);
            builder.Property(x => x.ProductId).UseIdentityColumn();
            builder.Property(x => x.ProductName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.PriceNew).IsRequired();
            builder.Property(x => x.PriceOld).IsRequired();
            builder.Property(x => x.ProductDescription).IsRequired();
            builder.Property(x => x.ShortDetails).IsRequired();

            builder.HasOne(x => x.productAction).WithOne(x => x.products).HasForeignKey<ProductActions>(x => x.ProductId);


        }
    }
}
