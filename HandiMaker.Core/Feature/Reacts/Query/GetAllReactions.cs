using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Reacts.Query
{

    public class GetAllReactionsDto
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PictureUrl { get; set; }
        public bool isFollowed { get; set; } = false;
    }

    public class GetAllReactionsModel : IRequest<BaseResponse<List<GetAllReactionsDto>>>
    {
        public int PostId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }
    public class GetAllReactionsHandler : BaseResponseHandler, IRequestHandler<GetAllReactionsModel, BaseResponse<List<GetAllReactionsDto>>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public GetAllReactionsHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }

        public async Task<BaseResponse<List<GetAllReactionsDto>>> Handle(GetAllReactionsModel request, CancellationToken cancellationToken)
        {
            var post = await _handiMakerDb.Posts
                .Include(p => p.ReactedUsers)
                .FirstOrDefaultAsync(p => p.Id == request.PostId);

            if (post is null)
                return Failed<List<GetAllReactionsDto>>(HttpStatusCode.NotFound, "Post not found");

            var authorizedUser = await _handiMakerDb.Users
                .Include(U => U.Following).FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));

            var Result = post.ReactedUsers.Select(RU => new GetAllReactionsDto
            {
                UserId = RU.Id,
                FirstName = RU.FirstName,
                LastName = RU.LastName,
                PictureUrl = RU.PictureUrl,
                isFollowed = authorizedUser != null && authorizedUser.Following.Any(F => F.FollowedId == RU.Id)
            }).ToList();
            return Success(Result);
        }
    }
}
