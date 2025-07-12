using HandiMaker.Data.Entities.ProductClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandiMaker.Infrastructure.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasMany(P => P.ProductColors)
                .WithOne(C => C.Product)
                .HasForeignKey(C => C.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(P => P.ProductPictures)
                .WithOne(C => C.Product)
                .HasForeignKey(C => C.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}