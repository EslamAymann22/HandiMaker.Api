using HandiMaker.Data.Helper;
using HandiMaker.Services.Services.implement;
using HandiMaker.Services.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HandiMaker.Services
{
    public static class ModuleServicesDependencies
    {

        public static IServiceCollection AddServicesDependencies(this IServiceCollection Service, IConfiguration configuration)
        {

            Service.AddScoped<IAuthenticationServices, AuthenticationServices>();
            Service.AddScoped<IEmailService, EmailService>();
            Service.AddScoped<ICacheServices, CacheServices>();
            Service.AddScoped<IResetPasswordServices, ResetPasswordServices>();
            Service.AddScoped<INotificationServices, NotificationServices>();

            var emailsettings = new EmailSettings();
            configuration.GetSection(nameof(EmailSettings)).Bind(emailsettings);
            Service.AddSingleton(emailsettings);
            Service.AddMemoryCache();

            return Service;

        }

    }
}
