using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace DoAn.Configurations
{
    public class ProductActionConfigurations : IEntityTypeConfiguration<ProductActions>
    {
        public void Configure(EntityTypeBuilder<ProductActions> builder)
        {
            builder.ToTable("ProductAction");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

        }
    }
}
