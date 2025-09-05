using HandiMaker.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace HandiMaker.Services.Services.Interface
{
    public interface IChatServices
    {
        Task<Message> SendMessageAsync(string fromId, string toId, string? content, IFormFile? file);
        Task<Chat> CreateChatAsync(string fromId, string toId);
        Task SendMessageWithHubAsync(Message message, string FromId, string ToId);
    }
}
