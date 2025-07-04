using HandiMaker.Data.Entities;
using HandiMaker.Data.Entities.PostClasses;
using HandiMaker.Data.Entities.ProductClasses;
using HandiMaker.Data.Entities.UserClassese;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HandiMaker.Infrastructure.DbContextData
{
    public class HandiMakerDbContext : IdentityDbContext<AppUser>
    {

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPicture> PostsPictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public HandiMakerDbContext(DbContextOptions<HandiMakerDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

    }
}
