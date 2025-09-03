
using Fas7niApp.Infrastructure;
using HandiMaker.Core;
using HandiMaker.Data.Entities;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AddToFav services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<HandiMakerDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //Options.UseSqlServer(builder.Configuration.GetConnectionString("PublicConnection"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<HandiMakerDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor()
                .AddInfrastructureDependencies()
                .AddCoreDependencies()
                .AddServicesDependencies(builder.Configuration)
                .AddJWTTokenConfigurations(builder.Configuration);


            var app = builder.Build();


            #region AutoDatabaseUpdate 
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = Services.GetRequiredService<HandiMakerDbContext>();

                await DbContext.Database.MigrateAsync();

                //using (var scope = app.Services.CreateScope())
                //{
                //    var _Fas7nyDbContext = scope.ServiceProvider.GetRequiredService<HandiMakerDbContext>();
                //    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                //    var _RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //    //await StoreContextSeeding.AddAllSeedingData(_RoleManager, _userManager, _Fas7nyDbContext);
                //}

            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error During Update database in Program\n");

            }
            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
