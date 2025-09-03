using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;

namespace HandiMaker.Services.Services.Interface
{
    public interface INotificationServices
    {

        public Task<bool> SendNotificationAsync(AppUser User, string Content, string NotifiedUserId, string RouteLink, NotifiType NotifiType);


    }
}
