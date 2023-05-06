using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable(nameof(Cart));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();


            builder.HasOne(x => x.SessUser).WithMany(x => x.Carts).HasForeignKey(x => x.SessionId);


        }
    }
}
