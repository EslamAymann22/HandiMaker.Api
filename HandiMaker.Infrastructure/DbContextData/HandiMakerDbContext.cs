using HandiMaker.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HandiMaker.Infrastructure.DbContextData
{
    public class HandiMakerDbContext : IdentityDbContext<AppUser>
    {

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
