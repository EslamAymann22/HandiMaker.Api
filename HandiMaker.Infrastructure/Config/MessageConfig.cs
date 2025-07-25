using HandiMaker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasMany(M => M.MessageDocs)
                .WithOne(D => D.Message)
                .HasForeignKey(D => D.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
