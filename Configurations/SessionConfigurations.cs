using DoAn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Configurations
{
    public class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Session");
            builder.HasKey(x => x.SessionId);
            builder.Property(x => x.SessionId).UseIdentityColumn();

            builder.HasOne(x => x.user).WithOne(x => x.session).HasForeignKey<Session>(x => x.Uid);
        }
    }
}
