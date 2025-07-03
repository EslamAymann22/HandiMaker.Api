using HandiMaker.Services.Services.implement;
using HandiMaker.Services.Services.Interface;
using Microsoft.Extensions.DependencyInjection;


namespace HandiMaker.Service
{
    public static class ModuleServicesDependencies
    {

        public static IServiceCollection AddServicesDependencies(this IServiceCollection Service)
        {

            Service.AddScoped<IAuthenticationServices, AuthenticationServices>();
            return Service;

        }

    }
}
