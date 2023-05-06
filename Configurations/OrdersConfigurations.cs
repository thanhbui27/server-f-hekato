using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;
using System.Reflection.Emit;

namespace DoAn.Configurations
{
    public class OrdersConfigurations : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable(nameof(Orders));
            builder.HasKey(x =>  x.OrderId);
            builder.Property(x => x.OrderId).UseIdentityColumn();

            builder.HasOne(x => x.users).WithMany(x => x.orders).HasForeignKey(x => x.Uid);

        }
    }
}
