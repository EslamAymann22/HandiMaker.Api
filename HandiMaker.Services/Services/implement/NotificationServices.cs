using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.Interface;

namespace HandiMaker.Services.Services.implement
{
    public class NotificationServices : INotificationServices
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public NotificationServices(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<bool> SendNotificationAsync(AppUser User, string Content, string NotifiedUserId, string RouteLink)
        {

            _handiMakerDb.Notifications.Add(new Notification
            {
                Content = Content,
                IsRead = false,
                NoteAt = DateTime.Now,
                UserId = NotifiedUserId,
                NotifiedUserId = User.Id,
                NotifiType = NotifiType.NewFollower,
                RouteLink = RouteLink
            });
            return await _handiMakerDb.SaveChangesAsync() > 1;

        }
    }
}
