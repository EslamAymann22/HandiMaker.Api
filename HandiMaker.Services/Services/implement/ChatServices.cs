using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Infrastructure.Hubs;
using HandiMaker.Services.Services.HelperStatic;
using HandiMaker.Services.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Services.Services.implement
{
    public class ChatServices : IChatServices
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<MyHub> _hubContext;
        private readonly INotificationServices _notificationServices;

        public ChatServices(HandiMakerDbContext handiMakerDb
            , IHttpContextAccessor httpContextAccessor
            , IHubContext<MyHub> hubContext
            , INotificationServices notificationServices)
        {
            this._handiMakerDb = handiMakerDb;
            this._httpContextAccessor = httpContextAccessor;
            this._hubContext = hubContext;
            this._notificationServices = notificationServices;
        }

        public async Task<Chat> CreateChatAsync(string FromId, string ToId)
        {
            var chat = new Data.Entities.Chat
            {
                FUserId = FromId,
                SUserId = ToId,
            };
            await _handiMakerDb.Chats.AddAsync(chat);
            await _handiMakerDb.SaveChangesAsync();
            return chat;
        }

        public async Task SendMessageWithHubAsync(Message message, string FromId, string ToId)
        {
            var FromConnections = _handiMakerDb.Connections.Where(c => c.UserId == FromId).Select(c => c.ConnectionId).ToList();
            var ToConnections = _handiMakerDb.Connections.Where(c => c.UserId == ToId).Select(c => c.ConnectionId).ToList();

            foreach (var connectionId in FromConnections)
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            if (ToConnections.Any())
            {
                foreach (var connectionId in ToConnections)
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                var user = await _handiMakerDb.Users.FindAsync(FromId);
                if (user is not null)
                {
                    var NotifiContent = $"You have a new message from {user.FirstName + " " + user.LastName} : \n {message.Content ?? ""}";
                    var RouteLinke = $"/api/Chat/GetChat?ChatId={message.Chat.Id}";
                    await _notificationServices.SendNotificationAsync(user, NotifiContent, ToId, RouteLinke, NotifiType.NewMessage);
                }
            }
        }

        public async Task<Message> SendMessageAsync(string FromId, string ToId, string? Content, IFormFile? file)
        {
            var CreatedAt = DateTime.Now; // to ignore waiting  
            var chat = await _handiMakerDb.Chats.Where(C => (C.FUserId == FromId && C.SUserId == ToId)
                                                || (C.FUserId == ToId && C.SUserId == FromId)).FirstOrDefaultAsync();

            if (chat is null)
                chat = await CreateChatAsync(FromId, ToId);

            string FileUrl = null;
            if (file is not null)
                FileUrl = DocumentServices.UploadFile(file, "ChatFiles", _httpContextAccessor);

            var message = new Message
            {
                ChatId = chat.Id,
                Content = Content,
                FileUrl = FileUrl,
                IsSeen = false,
                HasFile = FileUrl is not null,
                UserId = FromId,
                CreateAt = CreatedAt
            };
            await _handiMakerDb.Messages.AddAsync(message);
            await _handiMakerDb.SaveChangesAsync();
            chat.LastMessageId = message.Id;
            chat.NumberOfMessagesUnseen++;
            await _handiMakerDb.SaveChangesAsync();

            await SendMessageWithHubAsync(message, FromId, ToId);

            return message;
        }
    }
}
