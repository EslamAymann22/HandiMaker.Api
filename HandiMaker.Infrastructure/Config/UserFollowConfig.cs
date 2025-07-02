using HandiMaker.Data.Entities.UserClassese;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class UserFollowConfig : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.HasKey(UF => new { UF.FollowingId, UF.FollowerId });

            builder.HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uf => uf.Following)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(uf => uf.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
