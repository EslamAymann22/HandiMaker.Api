using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Comments.Query
{
    public class GetCommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string CommentOwnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HavChilds { get; set; }
    }

    public class GetPostCommentsModel : PaginationParams, IRequest<PaginatedResponse<GetCommentDto>>
    {

        public int PostId { get; set; }

    }


    public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsModel, PaginatedResponse<GetCommentDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetPostCommentsHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<PaginatedResponse<GetCommentDto>> Handle(GetPostCommentsModel request, CancellationToken cancellationToken)
        {
            var post = await _handiMakerDb.Posts.FirstOrDefaultAsync(P => P.Id == request.PostId);
            if (post is null)
                throw new Exception("This post is not found");

            var Qdata = _handiMakerDb.Comments.Where(C => C.PostId == request.PostId && !C.ParentId.HasValue).
                Include(C => C.CommentOwner).
                Select(C => new GetCommentDto
                {
                    CommentId = C.Id,
                    CommentOwnerId = C.CommentOwnerId,
                    Content = C.Content,
                    CreatedAt = C.CreatedAt,
                    HavChilds = C.NumOfChildren > 0,
                    FirstName = C.CommentOwner.FirstName,
                    LastName = C.CommentOwner.LastName,
                    PictureUrl = C.CommentOwner.PictureUrl
                }).AsQueryable().AsNoTracking();

            return await Qdata.ToPaginatedListAsync(request.PageNumber, request.PageSize);

        }
    }


}
