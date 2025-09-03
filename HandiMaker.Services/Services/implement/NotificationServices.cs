using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Infrastructure.Hubs;
using HandiMaker.Services.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace HandiMaker.Services.Services.implement
{
    public class NotificationServices : INotificationServices
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationServices(HandiMakerDbContext handiMakerDb, IHubContext<MyHub> hubContext)
        {
            this._handiMakerDb = handiMakerDb;
            this._hubContext = hubContext;
        }
        public async Task<bool> SendNotificationAsync(AppUser User, string Content, string NotifiedUserId, string RouteLink, NotifiType NotifiType)
        {
            var NewNotification = new Notification
            {
                Content = Content,
                IsRead = false,
                NoteAt = DateTime.Now,
                UserId = NotifiedUserId,
                NotifiedUserId = User.Id,
                NotifiType = NotifiType,
                RouteLink = RouteLink
            };
            _handiMakerDb.Notifications.Add(NewNotification);

            await _hubContext.Clients.User(NotifiedUserId).SendAsync("ReceiveNotification", NewNotification);

            return await _handiMakerDb.SaveChangesAsync() > 1;

        }
    }
}
