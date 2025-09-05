using HandiMaker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class ChatConfig : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasOne(C => C.FUser)
                  .WithMany()
                  .HasForeignKey(C => C.FUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(C => C.SUser)
                .WithMany()
                .HasForeignKey(C => C.SUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(C => C.Messages)
                .WithOne(M => M.Chat)
                .HasForeignKey(M => M.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(C => C.LastMessage)
                .WithOne()
                .HasForeignKey<Chat>(C => C.LastMessageId);
        }
    }
}
