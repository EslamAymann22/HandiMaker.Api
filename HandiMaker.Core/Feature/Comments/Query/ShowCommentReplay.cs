using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Comments.Query
{
    public class ShowCommentReplayModel : PaginationParams, IRequest<PaginatedResponse<GetCommentDto>>
    {
        public int CommentId { get; set; }
    }

    public class ShowCommentReplayHandler : IRequestHandler<ShowCommentReplayModel, PaginatedResponse<GetCommentDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public ShowCommentReplayHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<PaginatedResponse<GetCommentDto>> Handle(ShowCommentReplayModel request, CancellationToken cancellationToken)
        {
            var Qdata = _handiMakerDb.Comments.Where(C => C.Id == request.CommentId)
                .Include(C => C.Children).SelectMany(C => C.Children).Select(
                C => new GetCommentDto
                {
                    CommentId = C.Id,
                    CommentOwnerId = C.CommentOwnerId,
                    Content = C.Content,
                    CreatedAt = C.CreatedAt,
                    HavChilds = C.NumOfChildren > 0,
                    FirstName = C.CommentOwner.FirstName,
                    LastName = C.CommentOwner.LastName,
                    PictureUrl = C.CommentOwner.PictureUrl,
                }).AsQueryable().AsNoTracking();
            return await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
