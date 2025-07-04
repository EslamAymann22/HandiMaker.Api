using HandiMaker.Data.Entities.UserClassese;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class UserFollowConfig : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.HasKey(UF => new { UF.FollowedId, UF.FollowerId });

            builder.HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uf => uf.Followed)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(uf => uf.FollowedId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
