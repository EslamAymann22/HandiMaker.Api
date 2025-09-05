using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Chat.Query
{
    public class GetChatMessagesDto
    {
        public int MessageId { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public bool HasFile { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsSeen { get; set; }
    }

    public class GetChatMessagesModel : PaginationParams, IRequest<PaginatedResponse<GetChatMessagesDto>>
    {
        public int ChatId { get; set; }
        public string? UserId { get; set; }
    }

    public class GetChatMessagesHandler : IRequestHandler<GetChatMessagesModel, PaginatedResponse<GetChatMessagesDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetChatMessagesHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<PaginatedResponse<GetChatMessagesDto>> Handle(GetChatMessagesModel request, CancellationToken cancellationToken)
        {
            var chat = _handiMakerDb.Chats.Find(request.ChatId);
            if (request.UserId is null || chat is null || (chat.FUserId != request.UserId && chat.SUserId != request.UserId))
                return new PaginatedResponse<GetChatMessagesDto>(new List<GetChatMessagesDto>());

            var messagesQ = _handiMakerDb.Messages.Where(M => M.ChatId == request.ChatId)
                    .OrderByDescending(M => M.CreateAt)
                    .Select(M => new GetChatMessagesDto
                    {
                        MessageId = M.Id,
                        UserId = M.UserId,
                        Content = M.Content,
                        FileUrl = M.FileUrl,
                        HasFile = M.HasFile,
                        CreateAt = M.CreateAt,
                        IsSeen = M.IsSeen
                    }).AsQueryable().AsNoTracking();

            return await messagesQ.ToPaginatedListAsync(request.PageNumber, request.PageSize);

        }
    }


}
