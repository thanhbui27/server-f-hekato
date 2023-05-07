using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class CommentsProductsConfigutations : IEntityTypeConfiguration<CommentsProducts>
    {
        public void Configure(EntityTypeBuilder<CommentsProducts> builder)
        {
            builder.ToTable("CommentProduct");
            builder.HasKey(x => x.CommentsId);
            builder.Property(x => x.CommentsId).UseIdentityColumn();

            builder.HasOne(x => x.product).WithMany(x => x.comments).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.user).WithMany(x => x.commentsProducts).HasForeignKey(x => x.Uid);

        }
    }
}
