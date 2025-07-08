using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HandiMaker.Services
{
    public static class JWTTokenConfigurations
    {
        public static IServiceCollection AddJWTTokenConfigurations(this IServiceCollection Services, IConfiguration configuration)
        {

            Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(Options =>
               {
                   Options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = configuration["JWT:ValidIssuer"],
                       ValidateAudience = true,
                       ValidAudience = configuration["JWT:ValidAudience"],
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                   };
               }); // UserManager SignInManager RoleManager  


            return Services;

        }


    }
}
