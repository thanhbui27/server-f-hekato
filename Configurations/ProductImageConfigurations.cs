using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {

            builder.ToTable("ProductImages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.url_image).HasMaxLength(200).IsRequired(true);

            builder.HasOne(x => x.product).WithMany(x => x.GetsProductImage).HasForeignKey(x => x.ProductId);

        }
    }
}
