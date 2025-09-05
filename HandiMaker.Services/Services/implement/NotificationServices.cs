using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Infrastructure.Hubs;
using HandiMaker.Services.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace HandiMaker.Services.Services.implement
{
    public class NotificationServices : INotificationServices
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly IConfiguration _configuration;

        public NotificationServices(HandiMakerDbContext handiMakerDb, IHubContext<MyHub> hubContext, IConfiguration configuration)
        {
            this._handiMakerDb = handiMakerDb;
            this._hubContext = hubContext;
            this._configuration = configuration;
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
                RouteLink = $"{_configuration["BaseUrl"]}{RouteLink}"
            };
            _handiMakerDb.Notifications.Add(NewNotification);

            await _hubContext.Clients.User(NotifiedUserId).SendAsync("ReceiveNotification", NewNotification);

            return await _handiMakerDb.SaveChangesAsync() > 1;

        }
    }
}
