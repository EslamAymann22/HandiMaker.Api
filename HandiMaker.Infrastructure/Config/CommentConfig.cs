using HandiMaker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{

    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(C => C.Post)
                .WithMany(P => P.Comments)
                .HasForeignKey(C => C.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(C => C.Parent)
                .WithMany(C => C.Children)
                .HasForeignKey(C => C.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
