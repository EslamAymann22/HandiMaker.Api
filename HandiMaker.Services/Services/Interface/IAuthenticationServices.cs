using HandiMaker.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HandiMaker.Services.Services.Interface
{
    public interface IAuthenticationServices
    {
        Task<string> GetJWTTokenAsync(AppUser user, UserManager<AppUser> userManager);

    }
}
