using HandiMaker.Core.ResponseBase.Paginations;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HandiMaker.Core.Feature.Post.Query
{

    public class GetHomePostDto : GetPostDto
    {
        public bool FollowUser { get; set; } = false;
    }
    public class GetHomePostsModel : PaginationParams, IRequest<PaginatedResponse<GetHomePostDto>>
    {
        public string? AuthorizeEmail { get; set; }
    }

    public class GetHomePostsHandler : IRequestHandler<GetHomePostsModel, PaginatedResponse<GetHomePostDto>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetHomePostsHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<PaginatedResponse<GetHomePostDto>> Handle(GetHomePostsModel request, CancellationToken cancellationToken)
        {

            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));

            var QData = _handiMakerDb.Posts.Select(
                P => new GetHomePostDto
                {
                    PostId = P.Id,
                    FirstName = P.PostOwner.FirstName,
                    LastName = P.PostOwner.LastName,
                    Content = P.Content,
                    ImageUrl = P.PostOwner.PictureUrl,
                    CreatedAt = P.CreatedAt,
                    PostPictures = P.postPictures.Select(PP => PP.PicturUrl).ToList(),
                    numberOfLikes = P.ReactedUsers.Count,
                    numberOfComments = P.Comments.Count,
                    LoveIt = user != null && P.ReactedUsers.Any(RU => RU.Id == user.Id),
                    FollowUser = user != null && P.PostOwner.Followers.Any(F => F.FollowerId == user.Id)
                });
            QData = QData.OrderBy(P => P.LoveIt).ThenByDescending(P => P.FollowUser).ThenByDescending(P => P.CreatedAt);

            return await QData.ToPaginatedListAsync(request.PageNumber, request.PageSize);

        }
    }

}
