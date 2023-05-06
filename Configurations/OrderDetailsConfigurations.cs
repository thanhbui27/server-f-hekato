using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class OrderDetailsConfigurations : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.orders).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.products).WithMany(x => x.orderDetails).HasForeignKey(x => x.ProductId);
        }
    }
}
