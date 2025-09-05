using AutoMapper;
using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Chat.Query
{
    public class GetAllUserChatDto
    {

        public string ChatName { get; set; }
        public int ChatId { get; set; }
        public string? ChatImageUrl { get; set; }
        public int NumberOfUnseenMessages { get; set; }
        public string? LastMessage { get; set; }
        public string? FileUrl { get; set; }
        public DateTime? LastMessageTime { get; set; }


    }
    public class GetAllUserChatModel : PaginationParams, IRequest<PaginatedResponse<GetAllUserChatDto>>
    {
        public string? UserId { get; set; }
    }

    public class GetAllUserChatHandler : IRequestHandler<GetAllUserChatModel, PaginatedResponse<GetAllUserChatDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly IMapper _mapper;

        public GetAllUserChatHandler(HandiMakerDbContext handiMakerDb, IMapper mapper)
        {
            this._handiMakerDb = handiMakerDb;
            this._mapper = mapper;
        }

        public async Task<PaginatedResponse<GetAllUserChatDto>> Handle(GetAllUserChatModel request, CancellationToken cancellationToken)
        {

            if (request.UserId is null)
                return new PaginatedResponse<GetAllUserChatDto>(new List<GetAllUserChatDto>());

            var ChatsQ = _handiMakerDb.Chats.Where(C => C.FUserId == request.UserId || C.SUserId == request.UserId)
                                            .Include(C => C.LastMessage).Include(C => C.FUser).Include(C => C.SUser)
                                            .OrderByDescending(C => C.LastMessage.CreateAt)
                .Select(C => new GetAllUserChatDto
                {
                    ChatId = C.Id,
                    ChatImageUrl = C.FUserId == request.UserId ? C.SUser.PictureUrl : C.FUser.PictureUrl,
                    ChatName = C.FUserId == request.UserId ? C.SUser.FirstName + " " + C.SUser.LastName : C.FUser.FirstName + " " + C.FUser.LastName,
                    LastMessageTime = C.LastMessage != null ? C.LastMessage.CreateAt : null,
                    LastMessage = (C.LastMessage == null) ? null : C.LastMessage.Content ?? "",
                    NumberOfUnseenMessages = C.LastMessage.UserId == request.UserId ? 0 : C.NumberOfMessagesUnseen,
                    FileUrl = C.LastMessage.HasFile ? C.LastMessage.FileUrl : null
                }).AsQueryable().AsNoTracking();


            return await ChatsQ.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }

}
