using HandiMaker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {


            // Posts
            builder.HasMany(U => U.CreatedPosts)
                .WithOne(P => P.PostOwner)
                .HasForeignKey(U => U.PostOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(U => U.ReactedPosts)
                .WithMany(P => P.ReactedUsers);

            // Comments
            builder.HasMany(U => U.CreatedComments)
                .WithOne(C => C.CommentOwner)
                .HasForeignKey(C => C.CommentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications
            builder.HasMany(U => U.Notifications)
                .WithOne(N => N.User)
                .HasForeignKey(N => N.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Products
            builder.HasMany(U => U.Products)
                .WithOne(N => N.Owner)
                .HasForeignKey(N => N.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(U => U.FavProducts)
                .WithMany(P => P.FavAt);

            //Messages
            builder.HasMany(U => U.MessagesSent)
                .WithOne(N => N.Sender)
                .HasForeignKey(N => N.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(U => U.MessagesReceived)
                .WithOne(N => N.Receiver)
                .HasForeignKey(N => N.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
